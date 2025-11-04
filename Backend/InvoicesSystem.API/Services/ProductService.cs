using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using InvoicesSystem.API.Models.DTOs;
using InvoicesSystem.API.Models.Entities;
using InvoicesSystem.API.Services.Interfaces;
using InvoicesSystem.API.Persistence;
using Microsoft.EntityFrameworkCore;

namespace InvoicesSystem.API.Services;

public class ProductService : IProductService
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public ProductService(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<(IEnumerable<ProductDto> products, int total)> GetAllAsync(int page = 1, int pageSize = 10, string? search = null)
    {
        var query = _context.Products.AsQueryable();

        if (!string.IsNullOrEmpty(search))
        {
            query = query.Where(p => p.ProductName.Contains(search) || 
                                   (p.Description != null && p.Description.Contains(search)));
        }

        var total = await query.CountAsync();
        var products = await query
            .OrderBy(p => p.ProductName)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var productDtos = _mapper.Map<IEnumerable<ProductDto>>(products);
        return (productDtos, total);
    }

    public async Task<ProductDto?> GetByIdAsync(int id)
    {
        var product = await _context.Products
            .FirstOrDefaultAsync(p => p.IdProduct == id);
        
        return product == null ? null : _mapper.Map<ProductDto>(product);
    }

    public async Task<ProductDto> CreateAsync(CreateProductDto dto)
    {
        var product = new Product
        {
            CodeProduct = dto.CodeProduct,
            ProductName = dto.ProductName,
            Description = dto.Description,
            UnitPrice = dto.UnitPrice,
            UnitMeasure = dto.UnitMeasure,
            IsActive = dto.IsActive,
            CreatedAt = DateTime.UtcNow
        };

        // ✅ USAR ENTITY FRAMEWORK DIRECTAMENTE
        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        return _mapper.Map<ProductDto>(product);
    }

    public async Task<ProductDto?> UpdateAsync(int id, UpdateProductDto dto)
    {
        var product = await _context.Products
            .FirstOrDefaultAsync(p => p.IdProduct == id);
        
        if (product == null)
            return null;

        // Actualizar propiedades
        product.CodeProduct = dto.CodeProduct;
        product.ProductName = dto.ProductName;
        product.Description = dto.Description;
        product.UnitPrice = dto.UnitPrice;
        product.UnitMeasure = dto.UnitMeasure;
        product.IsActive = dto.IsActive;
        // ✅ NO usar UpdatedAt si no existe en la entidad

        await _context.SaveChangesAsync();
        return _mapper.Map<ProductDto>(product);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var product = await _context.Products
            .FirstOrDefaultAsync(p => p.IdProduct == id);
        
        if (product == null)
            return false;

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
        return true;
    }
}
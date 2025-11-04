using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using InvoicesSystem.API.Persistence;
using InvoicesSystem.API.Models.Entities;
using InvoicesSystem.API.Repositories.Interfaces;


namespace InvoicesSystem.API.Repositories;

public class ProductRepository : GenericRepository<Product>, IProductRepository
{
    public ProductRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<Product?> GetByIdWithTaxesAsync(int id)
    {
        return await _dbSet
            .Include(p => p.ProductTaxes!)
                .ThenInclude(pt => pt.Tax)
            .FirstOrDefaultAsync(p => p.IdProduct == id);
    }

    public async Task<Product?> GetByIdWithDetailsAsync(int id)
    {
        return await _dbSet
            .Include(p => p.ProductTaxes!)
                .ThenInclude(pt => pt.Tax)
            .FirstOrDefaultAsync(p => p.IdProduct == id);
    }

    public async Task<IEnumerable<Product>> GetAllWithTaxesAsync()
    {
        return await _dbSet
            .Include(p => p.ProductTaxes!)
                .ThenInclude(pt => pt.Tax)
            .ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetActiveProductsAsync()
    {
        return await _dbSet
            .Include(p => p.ProductTaxes!)
                .ThenInclude(pt => pt.Tax)
            .Where(p => p.IsActive)
            .ToListAsync();
    }

    public async Task<bool> ExistsByCodeAsync(string code, int? excludeId = null)
    {
        if (string.IsNullOrWhiteSpace(code))
            return false;

        var query = _dbSet.Where(p => p.CodeProduct == code);
        
        if (excludeId.HasValue)
        {
            query = query.Where(p => p.IdProduct != excludeId.Value);
        }
        
        return await query.AnyAsync();
    }

    public async Task<IEnumerable<Product>> SearchAsync(string searchTerm)
    {
        return await _dbSet
            .Include(p => p.ProductTaxes!)
                .ThenInclude(pt => pt.Tax)
            .Where(p => 
                (p.CodeProduct != null && p.CodeProduct.Contains(searchTerm)) ||
                p.ProductName!.Contains(searchTerm) ||
                (p.Description != null && p.Description.Contains(searchTerm))
            )
            .ToListAsync();
    }

    public async Task<(IEnumerable<Product> products, int total)> GetPagedAsync(int page, int pageSize, string? search = null)
    {
        var query = _dbSet
            .Include(p => p.ProductTaxes!)
                .ThenInclude(pt => pt.Tax)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(p => 
                p.ProductName!.Contains(search) || 
                p.CodeProduct!.Contains(search));
        }

        var total = await query.CountAsync();
        var products = await query
            .OrderBy(p => p.ProductName)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (products, total);
    }
}
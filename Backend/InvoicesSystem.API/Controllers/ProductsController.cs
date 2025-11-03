using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InvoicesSystem.API.Data;
using InvoicesSystem.API.Models.DTOs;
using InvoicesSystem.API.Models.Entities;

namespace InvoicesSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly AppDbContext _context;

    public ProductsController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/Products
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts(
        [FromQuery] string? search = null,
        [FromQuery] bool? isActive = null,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        var query = _context.Products
            .Include(p => p.ProductTaxes)
                .ThenInclude(pt => pt.Tax)
            .AsQueryable();

        // Filtros
        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(p =>
                (p.ProductName != null && p.ProductName.Contains(search)) ||
                (p.CodeProduct != null && p.CodeProduct.Contains(search)) ||
                (p.Description != null && p.Description.Contains(search))
            );
        }

        if (isActive.HasValue)
            query = query.Where(p => p.IsActive == isActive.Value);

        var total = await query.CountAsync();
        
        var products = await query
            .OrderBy(p => p.ProductName)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var result = products.Select(p => MapToDto(p)).ToList();

        Response.Headers.Add("X-Total-Count", total.ToString());
        Response.Headers.Add("X-Page", page.ToString());
        Response.Headers.Add("X-Page-Size", pageSize.ToString());

        return Ok(result);
    }

    // GET: api/Products/5
    [HttpGet("{id}")]
    public async Task<ActionResult<ProductDto>> GetProduct(int id)
    {
        var product = await _context.Products
            .Include(p => p.ProductTaxes)
                .ThenInclude(pt => pt.Tax)
            .FirstOrDefaultAsync(p => p.IdProduct == id);

        if (product == null)
            return NotFound(new { message = $"Producto con ID {id} no encontrado" });

        return Ok(MapToDto(product));
    }

    // POST: api/Products
    [HttpPost]
    public async Task<ActionResult<ProductDto>> CreateProduct(CreateProductDto dto)
    {
        // Validar que el código de producto sea único si se proporciona
        if (!string.IsNullOrWhiteSpace(dto.CodeProduct))
        {
            var existingProduct = await _context.Products
                .FirstOrDefaultAsync(p => p.CodeProduct == dto.CodeProduct);
            
            if (existingProduct != null)
                return BadRequest(new { message = "Ya existe un producto con ese código" });
        }

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

        // Agregar impuestos
        foreach (var taxId in dto.TaxIds)
        {
            var tax = await _context.Taxes.FindAsync(taxId);
            if (tax == null)
                return BadRequest(new { message = $"Impuesto con ID {taxId} no encontrado" });

            product.ProductTaxes.Add(new ProductTax
            {
                IdTax = taxId
            });
        }

        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        // Recargar con todas las relaciones
        var createdProduct = await _context.Products
            .Include(p => p.ProductTaxes)
                .ThenInclude(pt => pt.Tax)
            .FirstAsync(p => p.IdProduct == product.IdProduct);

        return CreatedAtAction(nameof(GetProduct), new { id = product.IdProduct }, MapToDto(createdProduct));
    }

    // PUT: api/Products/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(int id, CreateProductDto dto)
    {
        var product = await _context.Products
            .Include(p => p.ProductTaxes)
            .FirstOrDefaultAsync(p => p.IdProduct == id);
        
        if (product == null)
            return NotFound(new { message = $"Producto con ID {id} no encontrado" });

        // Validar código único si se cambió
        if (!string.IsNullOrWhiteSpace(dto.CodeProduct) && dto.CodeProduct != product.CodeProduct)
        {
            var existingProduct = await _context.Products
                .FirstOrDefaultAsync(p => p.CodeProduct == dto.CodeProduct);
            
            if (existingProduct != null)
                return BadRequest(new { message = "Ya existe un producto con ese código" });
        }

        product.CodeProduct = dto.CodeProduct;
        product.ProductName = dto.ProductName;
        product.Description = dto.Description;
        product.UnitPrice = dto.UnitPrice;
        product.UnitMeasure = dto.UnitMeasure;
        product.IsActive = dto.IsActive;

        // Actualizar impuestos
        _context.ProductTaxes.RemoveRange(product.ProductTaxes);
        
        foreach (var taxId in dto.TaxIds)
        {
            var tax = await _context.Taxes.FindAsync(taxId);
            if (tax == null)
                return BadRequest(new { message = $"Impuesto con ID {taxId} no encontrado" });

            product.ProductTaxes.Add(new ProductTax
            {
                IdProduct = id,
                IdTax = taxId
            });
        }

        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/Products/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var product = await _context.Products.FindAsync(id);
        
        if (product == null)
            return NotFound(new { message = $"Producto con ID {id} no encontrado" });

        // Verificar si está en facturas
        var hasInvoiceDetails = await _context.InvoiceDetails.AnyAsync(d => d.IdProduct == id);
        if (hasInvoiceDetails)
            return BadRequest(new { message = "No se puede eliminar el producto porque está en facturas" });

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // GET: api/Products/active
    [HttpGet("active")]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetActiveProducts()
    {
        var products = await _context.Products
            .Include(p => p.ProductTaxes)
                .ThenInclude(pt => pt.Tax)
            .Where(p => p.IsActive)
            .OrderBy(p => p.ProductName)
            .ToListAsync();

        return Ok(products.Select(p => MapToDto(p)));
    }

    private ProductDto MapToDto(Product product)
    {
        return new ProductDto
        {
            IdProduct = product.IdProduct,
            CodeProduct = product.CodeProduct,
            ProductName = product.ProductName ?? "",
            Description = product.Description,
            UnitPrice = product.UnitPrice,
            UnitMeasure = product.UnitMeasure ?? "",
            IsActive = product.IsActive,
            Taxes = product.ProductTaxes?.Select(pt => new ProductTaxDto
            {
                IdTax = pt.IdTax,
                TaxName = pt.Tax?.TaxName ?? "",
                TaxRate = pt.Tax?.TaxRate ?? 0
            }).ToList() ?? new List<ProductTaxDto>(),
            CreatedAt = product.CreatedAt
        };
    }
}
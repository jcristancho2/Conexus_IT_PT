using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using InvoicesSystem.API.Persistence;
using InvoicesSystem.API.Models.Entities;
using InvoicesSystem.API.Models.Enums;
using InvoicesSystem.API.Repositories.Interfaces;

namespace InvoicesSystem.API.Repositories;

public class InvoiceRepository : GenericRepository<Invoice>, IInvoiceRepository
{
    public InvoiceRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<Invoice?> GetByIdWithDetailsAsync(int id)
    {
        return await _dbSet
            .Include(i => i.Customer)
                .ThenInclude(c => c!.Address)
            .Include(i => i.Issuer)
            .Include(i => i.InvoiceDetails!) 
                .ThenInclude(id => id.Product)
            .Include(i => i.InvoiceDetails!)
                .ThenInclude(id => id.InvoiceDetailTaxes!)
                    .ThenInclude(idt => idt.Tax)
            .Include(i => i.InvoicePayments!)
                .ThenInclude(ip => ip.PaymentMethod)
            .FirstOrDefaultAsync(i => i.IdInvoice == id);
    }

    public async Task<IEnumerable<Invoice>> GetAllWithDetailsAsync()
    {
        return await _dbSet
            .Include(i => i.Customer)
            .Include(i => i.Issuer)
            .Include(i => i.InvoiceDetails)
            .Include(i => i.InvoicePayments)
            .OrderByDescending(i => i.InvoiceDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Invoice>> GetByCustomerIdAsync(int customerId)
    {
        return await _dbSet
            .Include(i => i.InvoiceDetails)
            .Include(i => i.InvoicePayments)
            .Where(i => i.IdCustomer == customerId)
            .OrderByDescending(i => i.InvoiceDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Invoice>> GetByStatusAsync(InvoiceStatus status)
    {
        return await _dbSet
            .Include(i => i.Customer)
            .Include(i => i.InvoiceDetails)
            .Where(i => i.Status == status)
            .OrderByDescending(i => i.InvoiceDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Invoice>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _dbSet
            .Include(i => i.Customer)
            .Include(i => i.InvoiceDetails)
            .Where(i => i.InvoiceDate >= startDate && i.InvoiceDate <= endDate)
            .OrderByDescending(i => i.InvoiceDate)
            .ToListAsync();
    }

    public async Task<string> GenerateInvoiceNumberAsync()
    {
        var lastInvoice = await _dbSet
            .OrderByDescending(i => i.IdInvoice)
            .FirstOrDefaultAsync();

        if (lastInvoice == null)
        {
            return $"INV-{DateTime.Now.Year}-00001";
        }

        var parts = lastInvoice.InvoiceNumber?.Split('-');
        if (parts != null && parts.Length == 3 && int.TryParse(parts[2], out int lastNumber))
        {
            var newNumber = lastNumber + 1;
            return $"INV-{DateTime.Now.Year}-{newNumber:D5}";
        }

        return $"INV-{DateTime.Now.Year}-00001";
    }

    public async Task<Dictionary<int, decimal>> GetProductSalesStatisticsAsync(DateTime? startDate = null, DateTime? endDate = null)
    {
        var query = _context.InvoiceDetails
            .Include(id => id.Invoice)
            .Include(id => id.Product)
            .AsQueryable();

        if (startDate.HasValue)
        {
            query = query.Where(id => id.Invoice!.InvoiceDate >= startDate.Value);
        }

        if (endDate.HasValue)
        {
            query = query.Where(id => id.Invoice!.InvoiceDate <= endDate.Value);
        }

        var statistics = await query
            .Where(id => id.Invoice!.Status == InvoiceStatus.Final)
            .GroupBy(id => id.IdProduct)
            .Select(g => new
            {
                ProductId = g.Key,
                TotalSales = g.Sum(id => id.Subtotal)
            })
            .ToDictionaryAsync(x => x.ProductId, x => x.TotalSales);

        return statistics;
    }

    public async Task<(IEnumerable<Invoice> invoices, int total)> GetPagedAsync(int page = 1, int pageSize = 10, string? search = null)
    {
        var query = _context.Invoices
            .Include(i => i.Customer)
            .Include(i => i.InvoiceDetails)
                .ThenInclude(d => d.Product)
            .Include(i => i.InvoicePayments)
                .ThenInclude(p => p.PaymentMethod)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(i => 
                i.InvoiceNumber!.Contains(search)
                || (i.Customer != null && (
                    (i.Customer.FirstName != null && i.Customer.FirstName.Contains(search))
                    || (i.Customer.LastName != null && i.Customer.LastName.Contains(search))
                    || i.Customer.IdentificationNumber.Contains(search)
                    || (i.Customer.BusinessName != null && i.Customer.BusinessName.Contains(search))
                ))
            );
        }

        var total = await query.CountAsync();
        var invoices = await query
            .OrderByDescending(i => i.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (invoices, total);
    }

    // CAMBIAR de AddAsync a AddAsyncCustom para evitar conflicto
    public async Task<Invoice> AddAsyncCustom(Invoice invoice)
    {
        try
        {
            Console.WriteLine($"=== DEBUG AddAsyncCustom ===");
            Console.WriteLine($"Invoice antes de Add: ID={invoice.IdInvoice}");
            
            // Agregar a EF Core
            var entityEntry = await _context.Invoices.AddAsync(invoice);
            Console.WriteLine($"Entity added to context");
            
            // Guardar cambios - ESTO ES CRUCIAL
            var changes = await _context.SaveChangesAsync();
            Console.WriteLine($"SaveChanges result: {changes} rows affected");
            
            // El ID debe haberse asignado automáticamente
            Console.WriteLine($"Invoice después de SaveChanges: ID={invoice.IdInvoice}");
            
            return invoice;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error en AddAsyncCustom: {ex.Message}");
            Console.WriteLine($"Inner Exception: {ex.InnerException?.Message}");
            throw;
        }
    }
}
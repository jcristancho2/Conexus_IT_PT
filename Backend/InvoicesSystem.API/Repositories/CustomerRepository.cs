using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using InvoicesSystem.API.Persistence;
using InvoicesSystem.API.Models.Entities;
using InvoicesSystem.API.Repositories.Interfaces;

namespace InvoicesSystem.API.Repositories;

public class CustomerRepository : GenericRepository<Customer>, ICustomerRepository
{
    public CustomerRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<Customer?> GetByIdWithDetailsAsync(int id)
    {
        return await _dbSet
            .Include(c => c.Address!)
                .ThenInclude(a => a.City!)
                    .ThenInclude(c => c.Department!)
                        .ThenInclude(d => d.Country)
            .Include(c => c.TypeIdentification)
            .Include(c => c.TaxRegime)
            .Include(c => c.TaxResponsibility)
            .Include(c => c.CustomerContacts)
            .FirstOrDefaultAsync(c => c.IdCustomer == id);
    }

    public async Task<IEnumerable<Customer>> GetAllWithDetailsAsync()
    {
        return await _dbSet
            .Include(c => c.Address!)
                .ThenInclude(a => a.City!)
                    .ThenInclude(c => c.Department!)
                        .ThenInclude(d => d.Country)
            .Include(c => c.TypeIdentification)
            .Include(c => c.TaxRegime)
            .Include(c => c.TaxResponsibility)
            .Include(c => c.CustomerContacts)
            .ToListAsync();
    }

    public async Task<bool> ExistsByIdentificationAsync(string identificationNumber, int? excludeId = null)
    {
        var query = _dbSet.Where(c => c.IdentificationNumber == identificationNumber);
        
        if (excludeId.HasValue)
        {
            query = query.Where(c => c.IdCustomer != excludeId.Value);
        }
        
        return await query.AnyAsync();
    }

    public async Task<IEnumerable<Customer>> SearchAsync(string searchTerm)
    {
        return await _dbSet
            .Include(c => c.Address)
            .Include(c => c.CustomerContacts)
            .Where(c => 
                c.IdentificationNumber.Contains(searchTerm) ||
                (c.FirstName != null && c.FirstName.Contains(searchTerm)) ||
                (c.LastName != null && c.LastName.Contains(searchTerm)) ||
                (c.BusinessName != null && c.BusinessName.Contains(searchTerm)) ||
                (c.CommercialName != null && c.CommercialName.Contains(searchTerm))
            )
            .ToListAsync();
    }

    public async Task<(IEnumerable<Customer> customers, int total)> GetPagedAsync(int page, int pageSize, string? search = null)
    {
        var query = _dbSet
            .Include(c => c.Address!)
                .ThenInclude(a => a.City!)
                    .ThenInclude(c => c.Department!)
                        .ThenInclude(d => d.Country)
            .Include(c => c.TypeIdentification)
            .Include(c => c.TaxRegime)
            .Include(c => c.TaxResponsibility)
            .Include(c => c.CustomerContacts)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(c => 
                c.Email!.Contains(search) ||
                c.IdentificationNumber!.Contains(search) ||
                (c.FirstName != null && c.FirstName.Contains(search)) ||
                (c.LastName != null && c.LastName.Contains(search)) ||
                (c.BusinessName != null && c.BusinessName.Contains(search)));
        }

        var total = await query.CountAsync();
        var customers = await query
            .OrderBy(c => c.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (customers, total);
    }
}
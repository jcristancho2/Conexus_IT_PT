using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.Linq.Expressions;
using InvoicesSystem.API.Models.Entities;

namespace InvoicesSystem.API.Repositories.Interfaces;

public interface ICustomerRepository : IGenericRepository<Customer>
{
    Task<Customer?> GetByIdWithDetailsAsync(int id);
    Task<(IEnumerable<Customer> customers, int total)> GetPagedAsync(int page, int pageSize, string? search = null);
    Task<bool> ExistsByIdentificationAsync(string identificationNumber, int? excludeId = null);
}
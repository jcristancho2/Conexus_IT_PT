using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.Linq.Expressions;
using InvoicesSystem.API.Models.Entities;

namespace InvoicesSystem.API.Repositories.Interfaces;

public interface IProductRepository : IGenericRepository<Product>
{
    Task<Product?> GetByIdWithDetailsAsync(int id);
    Task<(IEnumerable<Product> products, int total)> GetPagedAsync(int page, int pageSize, string? search = null);
    Task<bool> ExistsByCodeAsync(string code, int? excludeId = null);
}
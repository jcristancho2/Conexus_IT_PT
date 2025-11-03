using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.Linq.Expressions;
using InvoicesSystem.API.Models.Entities;
using InvoicesSystem.API.Models.Enums;

namespace InvoicesSystem.API.Repositories.Interfaces;

public interface IInvoiceRepository : IGenericRepository<Invoice>
{
    Task<Invoice?> GetByIdWithDetailsAsync(int id);
    Task<(IEnumerable<Invoice> invoices, int total)> GetPagedAsync(int page, int pageSize, string? search = null);
    Task<IEnumerable<Invoice>> GetByCustomerIdAsync(int customerId);
    Task<IEnumerable<Invoice>> GetByStatusAsync(InvoiceStatus status);
    Task<IEnumerable<Invoice>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<string> GenerateInvoiceNumberAsync();
    Task<Dictionary<int, decimal>> GetProductSalesStatisticsAsync(DateTime? startDate = null, DateTime? endDate = null);
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using InvoicesSystem.API.Models.DTOs;
using InvoicesSystem.API.Models.Enums;

namespace InvoicesSystem.API.Services.Interfaces;

public interface IInvoiceService
{
    Task<(IEnumerable<InvoiceDto> invoices, int total)> GetAllAsync(int page = 1, int pageSize = 10, string? search = null);
    Task<InvoiceDto?> GetByIdAsync(int id);
    Task<InvoiceDto> CreateAsync(CreateInvoiceDto dto);
    Task<InvoiceDto?> UpdateAsync(int id, UpdateInvoiceDto dto);
    Task<bool> DeleteAsync(int id);
    Task<IEnumerable<InvoiceDto>> GetInvoicesByCustomerAsync(int customerId);
    Task<IEnumerable<InvoiceDto>> GetInvoicesByStatusAsync(InvoiceStatus status);
    Task<IEnumerable<InvoiceDto>> GetInvoicesByDateRangeAsync(DateTime startDate, DateTime endDate);
}
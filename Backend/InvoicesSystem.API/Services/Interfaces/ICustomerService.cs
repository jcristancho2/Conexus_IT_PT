using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using InvoicesSystem.API.Models.DTOs;

namespace InvoicesSystem.API.Services.Interfaces;

public interface ICustomerService
{
    Task<(IEnumerable<CustomerDto> customers, int total)> GetAllAsync(int page = 1, int pageSize = 10, string? search = null);
    Task<CustomerDto?> GetByIdAsync(int id);
    Task<CustomerDto> CreateAsync(CreateCustomerDto dto);
    Task<CustomerDto?> UpdateAsync(int id, UpdateCustomerDto dto);
    Task<bool> DeleteAsync(int id);
}
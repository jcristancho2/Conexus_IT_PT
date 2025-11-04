using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using InvoicesSystem.API.Models.DTOs;

namespace InvoicesSystem.API.Services.Interfaces;

public interface IProductService
{
    Task<(IEnumerable<ProductDto> products, int total)> GetAllAsync(int page = 1, int pageSize = 10, string? search = null);
    Task<ProductDto?> GetByIdAsync(int id);
    Task<ProductDto> CreateAsync(CreateProductDto dto);
    Task<ProductDto?> UpdateAsync(int id, UpdateProductDto dto);
    Task<bool> DeleteAsync(int id);
}
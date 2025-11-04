using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InvoicesSystem.API.Models.DTOs;

namespace InvoicesSystem.API.Services.Interfaces;

public interface IDashboardService
{
    Task<DashboardDto> GetDashboardStatsAsync(DateTime? startDate = null, DateTime? endDate = null);
    Task<IEnumerable<ProductRevenueDto>> GetProductsRevenueAsync(DateTime? startDate = null, DateTime? endDate = null);
}
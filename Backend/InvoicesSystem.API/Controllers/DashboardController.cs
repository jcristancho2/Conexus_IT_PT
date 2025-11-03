using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using InvoicesSystem.API.Services.Interfaces;
using InvoicesSystem.API.Models.DTOs;
using InvoicesSystem.API.Models.Responses;

namespace InvoicesSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DashboardController : ControllerBase
{
    private readonly IDashboardService _dashboardService;

    public DashboardController(IDashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    [HttpGet("stats")]
    public async Task<ActionResult<ApiResponse<DashboardDto>>> GetStats(
        [FromQuery] DateTime? startDate = null,
        [FromQuery] DateTime? endDate = null)
    {
        var stats = await _dashboardService.GetDashboardStatsAsync(startDate, endDate);
        return Ok(ApiResponse<DashboardDto>.SuccessResponse(stats, "Estadísticas obtenidas exitosamente"));
    }

    [HttpGet("products-revenue")]
    public async Task<ActionResult<ApiResponse<IEnumerable<ProductRevenueDto>>>> GetProductsRevenue(
        [FromQuery] DateTime? startDate = null,
        [FromQuery] DateTime? endDate = null)
    {
        var data = await _dashboardService.GetProductsRevenueAsync(startDate, endDate);
        return Ok(ApiResponse<IEnumerable<ProductRevenueDto>>.SuccessResponse(
            data,
            "Total facturado por producto obtenido exitosamente"
        ));
    }
}
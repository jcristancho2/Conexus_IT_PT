using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InvoicesSystem.API.Data;
using InvoicesSystem.API.Models.DTOs;
using InvoicesSystem.API.Models.Enums;
using InvoicesSystem.API.Models.Entities;

namespace InvoicesSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DashboardController : ControllerBase
{
    private readonly AppDbContext _context;

    public DashboardController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/Dashboard
    [HttpGet]
    public async Task<ActionResult<DashboardDto>> GetDashboard(
        [FromQuery] DateTime? dateFrom = null,
        [FromQuery] DateTime? dateTo = null)
    {
        // Si no se proporcionan fechas, usar el mes actual
        var from = dateFrom ?? new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        var to = dateTo ?? DateTime.Now;

        // Total de ventas (solo facturas finalizadas)
        var totalSales = await _context.Invoices
            .Where(i => i.Status == InvoiceStatus.Final && 
                    i.InvoiceDate >= from && 
                    i.InvoiceDate <= to)
            .SumAsync(i => i.Total);

        // Total de facturas
        var totalInvoices = await _context.Invoices
            .Where(i => i.InvoiceDate >= from && i.InvoiceDate <= to)
            .CountAsync();

        // Total de clientes
        var totalCustomers = await _context.Customers.CountAsync();

        // Total de productos activos
        var totalProducts = await _context.Products
            .Where(p => p.IsActive)
            .CountAsync();

        // Facturas por estado (para gráfica de torta)
        var invoicesByStatus = await _context.Invoices
            .Where(i => i.InvoiceDate >= from && i.InvoiceDate <= to)
            .GroupBy(i => i.Status)
            .Select(g => new InvoiceStatusChartDto
            {
                Status = g.Key,
                Count = g.Count(),
                TotalAmount = g.Sum(i => i.Total)
            })
            .ToListAsync();

        // Últimas 5 facturas
        var recentInvoices = await _context.Invoices
            .Include(i => i.Customer)
            .Include(i => i.Issuer)
            .Include(i => i.InvoiceDetails)
                .ThenInclude(d => d.Product)
            .Include(i => i.InvoiceDetails)
                .ThenInclude(d => d.InvoiceDetailTaxes)
                    .ThenInclude(t => t.Tax)
            .Include(i => i.InvoicePayments)
                .ThenInclude(p => p.PaymentMethod)
            .OrderByDescending(i => i.InvoiceDate)
            .Take(5)
            .ToListAsync();

        // Top 5 productos más vendidos
        var topProducts = await _context.InvoiceDetails
            .Include(d => d.Product)
            .Include(d => d.Invoice)
            .Where(d => d.Invoice.Status == InvoiceStatus.Final &&
                    d.Invoice.InvoiceDate >= from &&
                    d.Invoice.InvoiceDate <= to)
            .GroupBy(d => new { d.IdProduct, d.Product!.ProductName })
            .Select(g => new TopProductDto
            {
                IdProduct = g.Key.IdProduct,
                ProductName = g.Key.ProductName ?? "",
                TotalSold = g.Sum(d => d.Quantity),
                TotalRevenue = g.Sum(d => d.Subtotal)
            })
            .OrderByDescending(p => p.TotalRevenue)
            .Take(5)
            .ToListAsync();

        var dashboard = new DashboardDto
        {
            TotalSales = totalSales,
            TotalInvoices = totalInvoices,
            TotalCustomers = totalCustomers,
            TotalProducts = totalProducts,
            InvoicesByStatus = invoicesByStatus,
            RecentInvoices = recentInvoices.Select(i => MapInvoiceToDto(i)).ToList(),
            TopProducts = topProducts
        };

        return Ok(dashboard);
    }

    // GET: api/Dashboard/chart
    [HttpGet("chart")]
    public async Task<ActionResult<IEnumerable<InvoiceStatusChartDto>>> GetInvoiceStatusChart(
        [FromQuery] DateTime? dateFrom = null,
        [FromQuery] DateTime? dateTo = null)
    {
        var from = dateFrom ?? new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        var to = dateTo ?? DateTime.Now;

        var chartData = await _context.Invoices
            .Where(i => i.InvoiceDate >= from && i.InvoiceDate <= to)
            .GroupBy(i => i.Status)
            .Select(g => new InvoiceStatusChartDto
            {
                Status = g.Key,
                Count = g.Count(),
                TotalAmount = g.Sum(i => i.Total)
            })
            .ToListAsync();

        return Ok(chartData);
    }

    // GET: api/Dashboard/sales-by-month
    [HttpGet("sales-by-month")]
    public async Task<ActionResult> GetSalesByMonth([FromQuery] int year = 0)
    {
        var targetYear = year == 0 ? DateTime.Now.Year : year;

        var salesByMonth = await _context.Invoices
            .Where(i => i.Status == InvoiceStatus.Final && 
                    i.InvoiceDate.Year == targetYear)
            .GroupBy(i => i.InvoiceDate.Month)
            .Select(g => new
            {
                Month = g.Key,
                TotalSales = g.Sum(i => i.Total),
                InvoiceCount = g.Count()
            })
            .OrderBy(x => x.Month)
            .ToListAsync();

        return Ok(salesByMonth);
    }

    // GET: api/Dashboard/top-customers
    [HttpGet("top-customers")]
    public async Task<ActionResult> GetTopCustomers(
        [FromQuery] int top = 10,
        [FromQuery] DateTime? dateFrom = null,
        [FromQuery] DateTime? dateTo = null)
    {
        var from = dateFrom ?? new DateTime(DateTime.Now.Year, 1, 1);
        var to = dateTo ?? DateTime.Now;

        var topCustomers = await _context.Invoices
            .Include(i => i.Customer)
            .Where(i => i.Status == InvoiceStatus.Final &&
                    i.InvoiceDate >= from &&
                    i.InvoiceDate <= to)
            .GroupBy(i => new
            {
                i.IdCustomer,
                CustomerName = i.Customer.PersonType == PersonType.Natural
                    ? i.Customer.FirstName + " " + i.Customer.LastName
                    : i.Customer.BusinessName ?? i.Customer.CommercialName
            })
            .Select(g => new
            {
                IdCustomer = g.Key.IdCustomer,
                CustomerName = g.Key.CustomerName,
                TotalPurchases = g.Sum(i => i.Total),
                InvoiceCount = g.Count()
            })
            .OrderByDescending(x => x.TotalPurchases)
            .Take(top)
            .ToListAsync();

        return Ok(topCustomers);
    }

    private InvoiceDto MapInvoiceToDto(Invoice invoice)
    {
        var customerName = invoice.Customer.PersonType == PersonType.Natural
            ? $"{invoice.Customer.FirstName} {invoice.Customer.LastName}"
            : invoice.Customer.BusinessName ?? invoice.Customer.CommercialName ?? "";

        return new InvoiceDto
        {
            IdInvoice = invoice.IdInvoice,
            InvoiceNumber = invoice.InvoiceNumber ?? "",
            InvoiceDate = invoice.InvoiceDate,
            DueDate = invoice.DueDate,
            Status = invoice.Status,
            Subtotal = invoice.Subtotal,
            TotalTax = invoice.TotalTax,
            Total = invoice.Total,
            Notes = invoice.Notes,
            IdCustomer = invoice.IdCustomer,
            CustomerName = customerName,
            CustomerIdentification = invoice.Customer.IdentificationNumber ?? "",
            IdIssuer = invoice.IdIssuer,
            IssuerBusinessName = invoice.Issuer.BusinessName ?? "",
            Details = invoice.InvoiceDetails?.Select(d => new InvoiceDetailDto
            {
                IdProduct = d.IdProduct,
                ProductName = d.Product?.ProductName ?? "",
                Quantity = d.Quantity,
                UnitPrice = d.UnitPrice,
                Discount = d.Discount,
                Subtotal = d.Subtotal,
                Description = d.Description,
                Taxes = d.InvoiceDetailTaxes?.Select(t => new InvoiceDetailTaxDto
                {
                    IdTax = t.IdTax,
                    TaxName = t.Tax?.TaxName ?? "",
                    TaxRate = t.Tax?.TaxRate ?? 0,
                    TaxBase = t.TaxBase,
                    TaxAmount = t.TaxAmount
                }).ToList() ?? new List<InvoiceDetailTaxDto>()
            }).ToList() ?? new List<InvoiceDetailDto>(),
            Payments = invoice.InvoicePayments?.Select(p => new InvoicePaymentDto
            {
                IdPaymentMethod = p.IdPaymentMethod,
                PaymentMethodName = p.PaymentMethod?.MethodName ?? "",
                Amount = p.Amount,
                PaymentDate = p.PaymentDate,
                Reference = p.Reference
            }).ToList() ?? new List<InvoicePaymentDto>(),
            CreatedAt = invoice.CreatedAt,
            UpdatedAt = invoice.UpdatedAt
        };
    }
}
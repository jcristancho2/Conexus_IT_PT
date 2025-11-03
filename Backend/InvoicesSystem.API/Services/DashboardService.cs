using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using InvoicesSystem.API.Persistence;
using InvoicesSystem.API.Models.DTOs;
using InvoicesSystem.API.Models.Enums;
using InvoicesSystem.API.Services.Interfaces;
using InvoicesSystem.API.Repositories.Interfaces;

namespace InvoicesSystem.API.Services;

public class DashboardService : IDashboardService
{
    private readonly AppDbContext _context;
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IProductRepository _productRepository;

    public DashboardService(
        AppDbContext context,
        IInvoiceRepository invoiceRepository,
        ICustomerRepository customerRepository,
        IProductRepository productRepository)
    {
        _context = context;
        _invoiceRepository = invoiceRepository;
        _customerRepository = customerRepository;
        _productRepository = productRepository;
    }

    public async Task<DashboardDto> GetDashboardStatsAsync(DateTime? startDate = null, DateTime? endDate = null)
    {
        var start = startDate ?? DateTime.UtcNow.AddMonths(-1);
        var end = endDate ?? DateTime.UtcNow;

        var invoices = await _context.Invoices
            // .Include(i => i.Customer) // ← COMENTADA TEMPORALMENTE
            .Include(i => i.InvoiceDetails!)
                .ThenInclude(d => d.Product)
            .Include(i => i.InvoiceDetails!)
                .ThenInclude(d => d.InvoiceDetailTaxes!)
            .Include(i => i.InvoicePayments!)
                .ThenInclude(p => p.PaymentMethod)
            .Where(i => i.InvoiceDate >= start && i.InvoiceDate <= end)
            .ToListAsync();

        // Estadísticas generales
        var totalInvoices = invoices.Count;
        var totalRevenue = invoices
            .Where(i => i.Status == InvoiceStatus.Final)
            .Sum(i => i.Total);
        var totalSales = invoices.Sum(i => i.Subtotal);

        var totalCustomers = await _context.Customers.CountAsync();
        var totalProducts = await _context.Products.CountAsync();

        // Facturas recientes (últimas 10)
        var recentInvoices = invoices
            .OrderByDescending(i => i.InvoiceDate)
            .Take(10)
            .Select(i => new RecentInvoiceDto
            {
                IdInvoice = i.IdInvoice,
                InvoiceNumber = i.InvoiceNumber ?? "",
                InvoiceDate = i.InvoiceDate,
                Total = i.Total,
                Status = i.Status.ToString(),  // ✅ CONVERTIR ENUM A STRING
                CustomerName = "Cliente Temporal" // VALOR TEMPORAL
            })
            .ToList();

        // Ventas por producto (cantidad)
        var productSalesByQuantity = invoices
            .Where(i => i.Status == InvoiceStatus.Final)
            .SelectMany(i => i.InvoiceDetails!)
            .GroupBy(d => d.Product!.ProductName)
            .Select(g => new PieChartData
            {
                Label = g.Key ?? "Sin nombre",
                Value = g.Sum(d => d.Quantity)
            })
            .OrderByDescending(p => p.Value)
            .Take(5)
            .ToList();

        // Ventas por producto (ingresos)
        var productSalesByRevenue = invoices
            .Where(i => i.Status == InvoiceStatus.Final)
            .SelectMany(i => i.InvoiceDetails!)
            .GroupBy(d => d.Product!.ProductName)
            .Select(g => new PieChartData
            {
                Label = g.Key ?? "Sin nombre",
                Value = g.Sum(d => d.Subtotal)
            })
            .OrderByDescending(p => p.Value)
            .Take(5)
            .ToList();

        // Datos de ventas por producto
        var totalProductRevenue = productSalesByRevenue.Sum(p => p.Value);
        var productSales = productSalesByRevenue
            .Select(p => new ProductSalesData
            {
                ProductId = 0, // Si necesitas el ID, debes incluirlo en el GroupBy
                ProductName = p.Label,
                TotalSales = p.Value,
                Percentage = totalProductRevenue > 0 ? (p.Value / totalProductRevenue) * 100 : 0
            })
            .ToList();

        // Facturas por estado
        var invoicesByStatus = invoices
            .GroupBy(i => i.Status)
            .Select(g => new InvoiceStatusChartDto
            {
                Status = g.Key.ToString(),
                Count = g.Count(),
                TotalAmount = g.Sum(i => i.Total)
            })
            .ToList();

        // Top 10 productos
        var topProducts = invoices
            .Where(i => i.Status == InvoiceStatus.Final)
            .SelectMany(i => i.InvoiceDetails!)
            .GroupBy(d => new { d.IdProduct, d.Product!.ProductName })
            .Select(g => new TopProductDto
            {
                IdProduct = g.Key.IdProduct,
                ProductName = g.Key.ProductName ?? "Sin nombre",
                TotalQuantity = g.Sum(d => d.Quantity),
                TotalRevenue = g.Sum(d => d.Subtotal)
            })
            .OrderByDescending(p => p.TotalRevenue)
            .Take(10)
            .ToList();

        return new DashboardDto
        {
            TotalInvoices = totalInvoices,
            TotalRevenue = totalRevenue,
            TotalSales = totalSales,  // ✅ AGREGADO
            TotalCustomers = totalCustomers,
            TotalProducts = totalProducts,
            RecentInvoices = recentInvoices,
            ProductSalesByQuantity = productSalesByQuantity,  // ✅ AGREGADO
            ProductSalesByRevenue = productSalesByRevenue,  // ✅ AGREGADO
            ProductSales = productSales,  // ✅ AGREGADO
            InvoicesByStatus = invoicesByStatus,  // ✅ AGREGADO
            TopProducts = topProducts,  // ✅ AGREGADO
            StartDate = start,  // ✅ AGREGADO
            EndDate = end  // ✅ AGREGADO
        };
    }

    public async Task<IEnumerable<ProductRevenueDto>> GetProductsRevenueAsync(
        DateTime? startDate = null,
        DateTime? endDate = null)
    {
        var query = _context.InvoiceDetails
            .Include(d => d.Invoice)
            .Include(d => d.Product)
            .Where(d => d.Invoice!.Status == InvoiceStatus.Final);

        if (startDate.HasValue)
            query = query.Where(d => d.Invoice!.InvoiceDate >= startDate.Value);

        if (endDate.HasValue)
            query = query.Where(d => d.Invoice!.InvoiceDate <= endDate.Value);

        return await query
            .GroupBy(d => new
            {
                d.IdProduct,
                d.Product!.CodeProduct,
                d.Product.ProductName
            })
            .Select(g => new ProductRevenueDto
            {
                ProductId = g.Key.IdProduct,
                ProductCode = g.Key.CodeProduct ?? "",
                ProductName = g.Key.ProductName ?? "",
                TotalQuantity = g.Sum(d => d.Quantity),
                TotalRevenue = g.Sum(d => d.Subtotal),
                InvoiceCount = g.Select(d => d.IdInvoice).Distinct().Count()
            })
            .OrderByDescending(p => p.TotalRevenue)
            .ToListAsync();
    }

    // Buscar las líneas que usan i.Customer y comentarlas temporalmente:

    /*
    public async Task<DashboardSummaryDto> GetDashboardSummaryAsync()
    {
        var totalInvoices = await _invoiceRepository.GetAllAsync();
        var customers = await _customerRepository.GetAllAsync();
        var products = await _productRepository.GetAllAsync();

        var summary = new DashboardSummaryDto
        {
            TotalInvoices = totalInvoices.Count(),
            TotalCustomers = customers.Count(),
            TotalProducts = products.Count(),
            // COMENTAR estas líneas temporalmente:
            // PendingInvoices = totalInvoices.Count(i => i.Status == InvoiceStatus.Draft),
            PendingInvoices = totalInvoices.Count(i => i.Status == InvoiceStatus.Draft),
            TotalRevenue = totalInvoices
                .Where(i => i.Status == InvoiceStatus.Final)
                .Sum(i => i.Total),
            // COMENTAR referencias a Customer temporalmente:
            // TopCustomers = totalInvoices
            //     .Where(i => i.Customer != null)
            //     .GroupBy(i => i.Customer!)
            //     .OrderByDescending(g => g.Sum(i => i.Total))
            //     .Take(5)
            //     .Select(g => new CustomerSummaryDto
            //     {
            //         IdCustomer = g.Key.IdCustomer,
            //         Name = g.Key.FirstName + " " + g.Key.LastName,
            //         TotalSpent = g.Sum(i => i.Total)
            //     })
            //     .ToList(),
            TopCustomers = new List<CustomerSummaryDto>(), // Lista vacía temporalmente
            
            // COMENTAR y simplificar esta parte también:
            // RecentInvoices = totalInvoices
            //     .Where(i => i.Customer != null)
            //     .OrderByDescending(i => i.CreatedAt)
            //     .Take(5)
            //     .Select(i => new RecentInvoiceDto
            //     {
            //         IdInvoice = i.IdInvoice,
            //         InvoiceNumber = i.InvoiceNumber ?? "",
            //         CustomerName = i.Customer!.FirstName + " " + i.Customer.LastName,
            //         Total = i.Total,
            //         Status = i.Status,
            //         CreatedAt = i.CreatedAt
            //     })
            //     .ToList()
            RecentInvoices = new List<RecentInvoiceDto>() // Lista vacía temporalmente
        };

        return summary;
    }
    */
}
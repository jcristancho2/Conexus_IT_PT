using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using InvoicesSystem.API.Models.Enums;

namespace InvoicesSystem.API.Models.DTOs;

public class DashboardDto
{
    // Estadísticas generales
    public int TotalInvoices { get; set; }
    public int TotalCustomers { get; set; }
    public int TotalProducts { get; set; }
    public decimal TotalSales { get; set; }
    public decimal TotalRevenue { get; set; }
    
    // Gráficas de productos
    public List<PieChartData> ProductSalesByQuantity { get; set; } = new();
    public List<PieChartData> ProductSalesByRevenue { get; set; } = new();
    public List<ProductSalesData> ProductSales { get; set; } = new();
    
    // Facturas por estado
    public List<InvoiceStatusChartDto> InvoicesByStatus { get; set; } = new();
    
    // Facturas recientes
    public List<RecentInvoiceDto> RecentInvoices { get; set; } = new();
    
    // Top productos
    public List<TopProductDto> TopProducts { get; set; } = new();
    
    // Rango de fechas
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}

public class PieChartData
{
    public string? Label { get; set; }
    public decimal Value { get; set; }
}

public class ProductSalesData
{
    public int ProductId { get; set; }
    public string? ProductName { get; set; }
    public decimal TotalSales { get; set; }
    public decimal Percentage { get; set; }
}

public class InvoiceStatusChartDto
{
    public string? Status { get; set; }
    public int Count { get; set; }
    public decimal TotalAmount { get; set; }
}

public class RecentInvoiceDto
{
    public int IdInvoice { get; set; }
    public string? InvoiceNumber { get; set; }
    public string? CustomerName { get; set; }
    public DateTime InvoiceDate { get; set; }
    public decimal Total { get; set; }
    public string? Status { get; set; }
}

public class TopProductDto
{
    public int IdProduct { get; set; }
    public string? ProductName { get; set; }
    public decimal TotalQuantity { get; set; }
    public decimal TotalRevenue { get; set; }
}
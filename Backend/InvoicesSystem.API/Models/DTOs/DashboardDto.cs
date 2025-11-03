using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using InvoicesSystem.API.Models.Enums;

namespace InvoicesSystem.API.Models.DTOs;

public class DashboardDto
{
    public decimal TotalSales { get; set; } // ventas totales
    public int TotalInvoices { get; set; } // total de facturas
    public int TotalCustomers { get; set; } // total de clientes
    public int TotalProducts { get; set; } // total de productos
    public List<InvoiceStatusChartDto> InvoicesByStatus { get; set; } = new(); // facturas por estado
    public List<InvoiceDto> RecentInvoices { get; set; } = new(); // últimas facturas
    public List<TopProductDto> TopProducts { get; set; } = new(); // top productos
}

public class InvoiceStatusChartDto
{
    public InvoiceStatus Status { get; set; } // estado de la factura
    public int Count { get; set; } // cantidad de facturas
    public decimal TotalAmount { get; set; } // monto total
}

public class TopProductDto
{
    public int IdProduct { get; set; } // id del producto
    public string? ProductName { get; set; } // nombre del producto
    public decimal TotalSold { get; set; } // total vendido
    public decimal TotalRevenue { get; set; } // ingresos totales
}
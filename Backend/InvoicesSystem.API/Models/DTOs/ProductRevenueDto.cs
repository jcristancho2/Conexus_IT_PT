using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvoicesSystem.API.Models.DTOs;

public class ProductRevenueDto
{
    public int ProductId { get; set; }
    public string? ProductCode { get; set; }
    public string? ProductName { get; set; }
    public decimal TotalQuantity { get; set; }
    public decimal TotalRevenue { get; set; }
    public int InvoiceCount { get; set; }
}
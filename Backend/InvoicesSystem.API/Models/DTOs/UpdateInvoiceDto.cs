using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using InvoicesSystem.API.Models.Enums;

namespace InvoicesSystem.API.Models.DTOs;

public class UpdateInvoiceDto
{
    public int IdCustomer { get; set; }
    
    public decimal SubtotalAmount { get; set; }
    
    public decimal TaxAmount { get; set; }
    
    public decimal TotalAmount { get; set; }
    
    public DateTime? DueDate { get; set; }

    public InvoiceStatus? Status { get; set; }

    [StringLength(500)]
    public string? Notes { get; set; }
    
    public List<CreateInvoiceDetailDto> Details { get; set; } = new();
}

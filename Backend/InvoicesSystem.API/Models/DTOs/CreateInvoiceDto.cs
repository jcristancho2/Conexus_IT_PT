using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using InvoicesSystem.API.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace InvoicesSystem.API.Models.DTOs;

public class CreateInvoiceDto
{
    [Required]
    public int IdCustomer { get; set; } // id del cliente
    
    [Required]
    public int IdInvoiceType { get; set; } // id del tipo de factura

    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "El subtotal debe ser mayor a 0")]
    public decimal SubtotalAmount { get; set; } // subtotal
    
    [Range(0, double.MaxValue, ErrorMessage = "El descuento no puede ser negativo")]
    public decimal DiscountAmount { get; set; } // descuento
    
    [Range(0, double.MaxValue, ErrorMessage = "Los impuestos no pueden ser negativos")]
    public decimal TaxAmount { get; set; } // impuestos
    
    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "El total debe ser mayor a 0")]
    public decimal TotalAmount { get; set; } // total
    
    public string? Notes { get; set; } // notas adicionales
    
    public DateTime? DueDate { get; set; } // fecha de vencimiento
    
    [Required]
    [MinLength(1, ErrorMessage = "La factura debe tener al menos un detalle")]
    public List<CreateInvoiceDetailDto> Details { get; set; } = new(); // detalles de la factura
    
    public List<CreateInvoicePaymentDto>? Payments { get; set; } = new(); // pagos de la factura
}




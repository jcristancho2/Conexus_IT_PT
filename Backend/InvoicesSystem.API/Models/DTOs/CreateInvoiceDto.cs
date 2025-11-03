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
    public int IdIssuer { get; set; } // id del emisor

    [Required]
    [StringLength(50)]
    public string? InvoiceNumber { get; set; } // numero de la factura
    
    [Required]
    public DateTime InvoiceDate { get; set; } // fecha de la factura

    public DateTime? DueDate { get; set; } // fecha de vencimiento
    
    [StringLength(500)]
    public string? Notes { get; set; } // notas adicionales
    
    [Required]
    [MinLength(1, ErrorMessage = "La factura debe tener al menos un detalle")]
    public List<CreateInvoiceDetailDto> Details { get; set; } = new(); // detalles de la factura
    
    public List<CreateInvoicePaymentDto> Payments { get; set; } = new(); // pagos de la factura
}

public class CreateInvoiceDetailDto
{
    [Required]
    public int IdProduct { get; set; } // id del producto
    
    [Required]
    [Range(0.0001, double.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0")]
    public decimal Quantity { get; set; } // cantidad
    
    [Required]
    [Range(0, double.MaxValue, ErrorMessage = "El precio unitario no puede ser negativo")]
    public decimal UnitPrice { get; set; } // precio unitario
    
    [Range(0, double.MaxValue, ErrorMessage = "El descuento no puede ser negativo")]
    public decimal Discount { get; set; } = 0; // descuento
    
    [StringLength(200)]
    public string? Description { get; set; } // descripcion
    
    public List<int> TaxIds { get; set; } = new(); // ids de los impuestos aplicados
}

public class CreateInvoicePaymentDto
{
    [Required]
    public int IdPaymentMethod { get; set; } // id del metodo de pago
    
    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "El monto debe ser mayor a 0")]
    public decimal Amount { get; set; } // monto
    
    [Required]
    public DateTime PaymentDate { get; set; } // fecha de pago

    [StringLength(100)]
    public string? Reference { get; set; } // referencia
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InvoicesSystem.API.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace InvoicesSystem.API.Models.DTOs
{
    public class CreateInvoiceDetailDto
{
    [Required]
    public int IdProduct { get; set; } // id del producto
    
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0")]
    public int Quantity { get; set; } // cantidad
    
    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "El precio unitario debe ser mayor a 0")]
    public decimal UnitPrice { get; set; } // precio unitario
    
    [Range(0, double.MaxValue, ErrorMessage = "El descuento no puede ser negativo")]
    public decimal DiscountAmount { get; set; } // descuento
    
    [Range(0, double.MaxValue, ErrorMessage = "Los impuestos no pueden ser negativos")]
    public decimal TaxAmount { get; set; } // impuestos
    
    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "El total debe ser mayor a 0")]
    public decimal TotalAmount { get; set; } // total
}
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;

namespace InvoicesSystem.API.Models.DTOs;

public class CreateProductDto
{
    public string? CodeProduct { get; set; } // codigo del producto
    
    [Required(ErrorMessage = "ProductName es requerido")]
    public string ProductName { get; set; } = string.Empty; // nombre del producto

    public string? Description { get; set; } // descripcion del producto

    [Required(ErrorMessage = "UnitPrice es requerido")]
    [Range(0.01, double.MaxValue, ErrorMessage = "UnitPrice debe ser mayor a 0")]
    public decimal UnitPrice { get; set; } // precio unitario
    
    [Required(ErrorMessage = "UnitMeasure es requerido")]
    public string UnitMeasure { get; set; } = string.Empty; // unidad de medida

    public bool IsActive { get; set; } = true; // estado del producto
    
    public List<int> TaxIds { get; set; } = new(); // ids de los impuestos aplicados
}
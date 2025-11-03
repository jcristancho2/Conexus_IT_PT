using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;

namespace InvoicesSystem.API.Models.DTOs;

public class CreateProductDto
{
    [StringLength(50)]
    public string? CodeProduct { get; set; } // codigo del producto
    
    [Required]
    [StringLength(100)]
    public string? ProductName { get; set; } // nombre del producto

    public string? Description { get; set; } // descripcion del producto

    [Required]
    [Range(0, double.MaxValue)]
    public decimal UnitPrice { get; set; } // precio unitario
    
    [Required]
    [StringLength(20)]
    public string? UnitMeasure { get; set; } // unidad de medida

    public bool IsActive { get; set; } = true; // estado del producto
    
    public List<int> TaxIds { get; set; } = new(); // ids de los impuestos aplicados
}
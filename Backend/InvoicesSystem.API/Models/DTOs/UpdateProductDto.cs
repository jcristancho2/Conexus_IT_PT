using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;

namespace InvoicesSystem.API.Models.DTOs;

public class UpdateProductDto
{
    public string? CodeProduct { get; set; }

    [Required(ErrorMessage = "ProductName es requerido")]
    public string ProductName { get; set; } = string.Empty;

    public string? Description { get; set; }

    [Required(ErrorMessage = "UnitPrice es requerido")]
    [Range(0.01, double.MaxValue, ErrorMessage = "UnitPrice debe ser mayor a 0")]
    public decimal UnitPrice { get; set; }

    [Required(ErrorMessage = "UnitMeasure es requerido")]
    public string UnitMeasure { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvoicesSystem.API.Models.DTOs;

public class ProductDto
{
    public int IdProduct { get; set; } // id del producto
    public string? CodeProduct { get; set; } // código del producto
    public string? ProductName { get; set; } // nombre del producto
    public string? Description { get; set; } // descripción del producto
    public decimal UnitPrice { get; set; } // precio unitario
    public string? UnitMeasure { get; set; } // unidad de medida
    public bool IsActive { get; set; } // estado del producto
    public List<ProductTaxDto> Taxes { get; set; } = new(); // lista de impuestos
    public DateTime CreatedAt { get; set; } // fecha de creación
}

public class ProductTaxDto
{
    public int IdTax { get; set; } // id del impuesto
    public string? TaxName { get; set; } // nombre del impuesto
    public decimal TaxRate { get; set; } // tasa del impuesto
}
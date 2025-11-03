using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvoicesSystem.API.Models.DTOs
{
    public class InvoiceDetailDto
    {
        public int IdProduct { get; set; } // id del producto
        public string? ProductName { get; set; } // nombre del producto
        public decimal Quantity { get; set; } // cantidad
        public decimal UnitPrice { get; set; } // precio unitario
        public decimal Discount { get; set; } // descuento
        public decimal Subtotal { get; set; } // subtotal
        public string? Description { get; set; } // descripcion
        public List<InvoiceDetailTaxDto> Taxes { get; set; } = new(); // impuestos aplicados
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvoicesSystem.API.Models.DTOs
{
    public class InvoiceDetailTaxDto
    {
        public int IdTax { get; set; } // id del impuesto
        public string? TaxName { get; set; } // nombre del impuesto
        public decimal TaxRate { get; set; } // tasa del impuesto
        public decimal TaxBase { get; set; } // base del impuesto
        public decimal TaxAmount { get; set; } // monto del impuesto
    }
}
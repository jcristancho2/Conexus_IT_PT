using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvoicesSystem.API.Models.DTOs
{
    public class InvoicePaymentDto 
{
    public int IdPaymentMethod { get; set; } // id del metodo de pago
    public string? PaymentMethodName { get; set; } // nombre del metodo de pago
    public decimal Amount { get; set; } // monto
    public DateTime PaymentDate { get; set; } // fecha de pago
    public string? Reference { get; set; } // referencia
}
}   
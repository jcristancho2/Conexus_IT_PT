using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InvoicesSystem.API.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace InvoicesSystem.API.Models.DTOs
{
    public class CreateInvoicePaymentDto
    {
        [Required]
        public int IdPaymentMethod { get; set; } // id del metodo de pago

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal Amount { get; set; } // monto

        [Required]
        public DateTime PaymentDate { get; set; } = DateTime.UtcNow; // fecha de pago

        [StringLength(100)]
        public string? Reference { get; set; } // referencia
    }
}
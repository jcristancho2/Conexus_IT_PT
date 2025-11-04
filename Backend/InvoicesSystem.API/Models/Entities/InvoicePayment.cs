using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InvoicesSystem.API.Models.Entities;

[Table("invoice_payment")]
public class InvoicePayment
{
    [Key]
    [Column("id_invoice", Order = 0)]
    public int IdInvoice { get; set; }

    [Key]
    [Column("id_payment_method", Order = 1)]
    public int IdPaymentMethod { get; set; }

    [Required]
    [Column("amount", TypeName = "decimal(18,2)")]
    public decimal Amount { get; set; }

    [Required]
    [Column("payment_date")]
    public DateTime PaymentDate { get; set; }

    [Column("reference")]
    [StringLength(100)]
    public string? Reference { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }

    // Navigation properties
    [ForeignKey(nameof(IdInvoice))]
    public virtual Invoice? Invoice { get; set; }

    [ForeignKey(nameof(IdPaymentMethod))]
    public virtual PaymentMethod? PaymentMethod { get; set; }
}
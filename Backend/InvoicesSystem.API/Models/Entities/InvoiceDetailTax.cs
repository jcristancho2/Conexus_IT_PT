using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InvoicesSystem.API.Models.Entities;

[Table("invoice_detail_tax")]
public class InvoiceDetailTax
{
    [Key, Column("id_invoice", Order = 0)]
    public int IdInvoice { get; set; }

    [Key, Column("id_product", Order = 1)]
    public int IdProduct { get; set; }

    [Key, Column("id_tax", Order = 2)]
    public int IdTax { get; set; }

    [Required]
    [Column("tax_base", TypeName = "decimal(18,2)")]
    public decimal TaxBase { get; set; }

    [Required]
    [Column("tax_amount", TypeName = "decimal(18,2)")]
    public decimal TaxAmount { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }

    // Navigation properties
    [ForeignKey($"{nameof(IdInvoice)},{nameof(IdProduct)}")]
    public virtual InvoiceDetail InvoiceDetail { get; set; } = null!;

    [ForeignKey(nameof(IdTax))]
    public virtual Tax Tax { get; set; } = null!;
}
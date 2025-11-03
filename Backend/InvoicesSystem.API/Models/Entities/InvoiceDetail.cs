using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InvoicesSystem.API.Models.Entities;

[Table("invoice_detail")]
public class InvoiceDetail
{
    [Key]
    [Column("id_invoice", Order = 0)]
    public int IdInvoice { get; set; }

    [Key]
    [Column("id_product", Order = 1)]
    [ForeignKey(nameof(Product))]
    public int IdProduct { get; set; }

    [Required]
    [Column("quantity", TypeName = "decimal(18,2)")]
    public decimal Quantity { get; set; }

    [Required]
    [Column("unit_price", TypeName = "decimal(18,2)")]
    public decimal UnitPrice { get; set; }

    [Required]
    [Column("discount", TypeName = "decimal(18,2)")]
    public decimal Discount { get; set; } = 0;

    [Required]
    [Column("subtotal", TypeName = "decimal(18,2)")]
    public decimal Subtotal { get; set; }

    [StringLength(200)]
    [Column("description")]
    public string? Description { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }

    // propiedades de navegacion
    [ForeignKey(nameof(Invoice))]
    public virtual Invoice Invoice { get; set; } = null!;
    public virtual Product Product { get; set; } = null!;

    public virtual ICollection<InvoiceDetailTax> InvoiceDetailTaxes { get; set; } = new List<InvoiceDetailTax>();
}
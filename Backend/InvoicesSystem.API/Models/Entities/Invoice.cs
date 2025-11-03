using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using InvoicesSystem.API.Models.Enums;

namespace InvoicesSystem.API.Models.Entities;

[Table("invoice")]
public class Invoice
{
    [Key]
    [Column("id_invoice")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // ← IMPORTANTE
    public int IdInvoice { get; set; }

    [Required]
    [Column("id_customer")]
    public int IdCustomer { get; set; }

    [Column("id_issuer")]
    public int? IdIssuer { get; set; }

    [Required]
    [StringLength(50)]
    [Column("invoice_number")]
    public string? InvoiceNumber { get; set; }

    [Required]
    [Column("invoice_date")]
    public DateTime InvoiceDate { get; set; }

    [Column("due_date")]
    public DateTime? DueDate { get; set; }

    [Required]
    [Column("status")]
    public InvoiceStatus Status { get; set; }

    [Required]
    [Column("subtotal", TypeName = "decimal(18,2)")]
    public decimal Subtotal { get; set; }

    [Required]
    [Column("total_tax", TypeName = "decimal(18,2)")]
    public decimal TotalTax { get; set; }

    [Required]
    [Column("total", TypeName = "decimal(18,2)")]
    public decimal Total { get; set; }

    [StringLength(500)]
    [Column("notes")]
    public string? Notes { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }

    // Propiedades de navegación - COMENTADAS TEMPORALMENTE
    // public virtual Customer? Customer { get; set; }
    // public virtual Issuer? Issuer { get; set; }
    
    // Estas SÍ las dejamos
    public virtual ICollection<InvoiceDetail> InvoiceDetails { get; set; } = new List<InvoiceDetail>();
    public virtual ICollection<InvoicePayment> InvoicePayments { get; set; } = new List<InvoicePayment>();
}
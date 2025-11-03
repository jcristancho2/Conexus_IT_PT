using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InvoicesSystem.API.Models.Entities
{
    [Table("issuer")]
    public class Issuer
    {
    [Key]
    [Column("id_issuer")]
    public int IdIssuer { get; set; }

    [Required]
    [Column("id_address")]
    [ForeignKey(nameof(Address))]
    public int IdAddress { get; set; }

    [Required]
    [Column("id_tax_regime")]
    [ForeignKey(nameof(TaxRegime))]
    public int IdTaxRegime { get; set; }

    [Required]
    [Column("id_tax_responsibility")]
    [ForeignKey(nameof(TaxResponsibility))]
    public int IdTaxResponsibility { get; set; }

    [Required]
    [StringLength(50)]
    [Column("identification_number")]
    public string? IdentificationNumber { get; set; }

    [Required]
    [StringLength(200)]
    [Column("business_name")]
    public string? BusinessName { get; set; }

    [StringLength(100)]
    [Column("commercial_name")]
    public string? CommercialName { get; set; }

    [StringLength(100)]
    [Column("email")]
    public string? Email { get; set; }

    [StringLength(20)]
    [Column("phone")]
    public string? Phone { get; set; }

    [StringLength(100)]
    [Column("website")]
    public string? Website { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }

    // properties de navegación
    public virtual Address? Address { get; set; }
    public virtual TaxRegime? TaxRegime { get; set; }
    public virtual TaxResponsibility? TaxResponsibility { get; set; }

    public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
        
    }
}
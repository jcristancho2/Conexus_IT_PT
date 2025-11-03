using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InvoicesSystem.API.Models.Entities
{
    [Table("tax")]
    public class Tax
    {
        [Key]
        [Column("id_tax")]
        public int IdTax { get; set; }

        [Required]
        [Column("tax_name")]
        [StringLength(100)]
        public string? TaxName { get; set; }

        [Required]
        [Column("tax_rate", TypeName = "numeric(7,4)")]
        public decimal TaxRate { get; set; }

        // propiedad de navegacion
        public virtual ICollection<ProductTax> ProductTaxes { get; set; } = new List<ProductTax>();
        public virtual ICollection<InvoiceDetailTax> InvoiceDetailTaxes { get; set; } = new List<InvoiceDetailTax>();
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InvoicesSystem.API.Models.Entities
{
    [Table("product")]
    public class Product
    {
        [Key]
        [Column("id_product")]
        public int IdProduct { get; set; }

        [Column("code_product")]
        [StringLength(50)]
        public string? CodeProduct { get; set; }

        [Required]
        [Column("product_name")]
        [StringLength(100)]
        public string? ProductName { get; set; }

        [Column("description")]
        public string? Description { get; set; }

        [Required]
        [Column("unit_price", TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }

        [Required]
        [Column("unit_measure")]
        [StringLength(20)]
        public string? UnitMeasure { get; set; }

        [Column("is_active")]
        public bool IsActive { get; set; } = true;

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // propiedad de navegacion
        public virtual ICollection<ProductTax> ProductTaxes { get; set; } = new List<ProductTax>();
        public virtual ICollection<InvoiceDetail> InvoiceDetails { get; set; } = new List<InvoiceDetail>();
    }
}
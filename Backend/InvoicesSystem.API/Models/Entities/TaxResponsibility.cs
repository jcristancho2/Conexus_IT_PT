using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InvoicesSystem.API.Models.Entities
{
    [Table("tax_responsibility")]
    public class TaxResponsibility
    {
        [Key]
        [Column("id_tax_responsibility")]
        public int IdTaxResponsibility { get; set; }

        [Required]
        [Column("code")]
        [StringLength(50)]
        public string? Code { get; set; }

        [Column("description")]
        [StringLength(150)]
        public string? Description { get; set; }

        // propiedad de navegacion
        public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();
        public virtual ICollection<Issuer> Issuers { get; set; } = new List<Issuer>();
    }
}
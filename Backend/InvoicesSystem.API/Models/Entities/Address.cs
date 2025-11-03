using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InvoicesSystem.API.Models.Entities
{
    [Table("address")]
    public class Address
    {
        [Key]
        [Column("id_address")]
        public int IdAddress { get; set; }

        [Required]
        [Column("full_address")]
        [StringLength(200)]
        public string? FullAddress { get; set; }

        [Required]
        [Column("id_city")]
        [ForeignKey(nameof(City))]
        public int IdCity { get; set; }
    
        // propiedad de navegacion
        public virtual City? City { get; set; }
        public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();
        public virtual ICollection<Issuer> Issuers { get; set; } = new List<Issuer>();
    }
}
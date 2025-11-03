using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InvoicesSystem.API.Models.Entities
{
    [Table("type_identification")]
    public class TypeIdentification
    {
        [Key]
        [Column("id_type_identification")]
        public int IdTypeIdentification { get; set; }

        [Required]
        [Column("code")]
        [StringLength(20)]
        public string? Code { get; set; }

        [Required]
        [Column("description")]
        [StringLength(100)]
        public string? Description { get; set; }

        // propiedad de navegacion
        public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();
    }
}
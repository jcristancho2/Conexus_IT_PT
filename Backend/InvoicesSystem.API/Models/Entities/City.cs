using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InvoicesSystem.API.Models.Entities
{
    [Table("city")]
    public class City
    {
        [Key]
        [Column("id_city")]
        public int IdCity { get; set; }

        [Required]
        [Column("name_city")]
        [StringLength(100)]
        public string? NameCity { get; set; }

        [Required]
        [Column("id_department")]
        [ForeignKey(nameof(Department))]
        public int IdDepartment { get; set; }

        // propiedad de navegacion
        public virtual Department? Department { get; set; }
        public virtual ICollection<Address> Addresses { get; set; } = new HashSet<Address>();
    }
}
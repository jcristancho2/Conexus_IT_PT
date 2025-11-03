using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InvoicesSystem.API.Models.Entities
{
    [Table("department")]
    public class Department
    {
        [Key]
        [Column("id_department")]
        public int IdDepartment { get; set; }

        [Required]
        [Column("name_department")]
        [StringLength(100)]
        public string? NameDepartment { get; set; }

        [Required]
        [Column("id_country")]
        [ForeignKey(nameof(Country))]
        public int IdCountry { get; set; }

        // propiedad de navegacion
        public virtual Country? Country { get; set; }
        public virtual ICollection<City> Cities { get; set; } = new HashSet<City>();
    }
}
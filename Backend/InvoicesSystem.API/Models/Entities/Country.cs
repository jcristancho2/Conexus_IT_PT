using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace InvoicesSystem.API.Models.Entities
{
    [Table("country")]
    public class Country
    {
        [Key]
        [Column("id_country")]
        public int IdCountry { get; set; }
        
        [Required]
        [Column("cod_country")]
        [StringLength(2)]
        public string? Cod_country { get; set; }
        
        [Required]
        [Column("name_country")]
        [StringLength(100)]
        public string? Name_country { get; set; }

        // propiedad de navegacion
        public virtual ICollection<Departament> Departaments { get; set; } = new HashSet<Departament>();
    }
}
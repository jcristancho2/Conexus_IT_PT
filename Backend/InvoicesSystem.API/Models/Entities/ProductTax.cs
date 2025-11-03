using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InvoicesSystem.API.Models.Entities
{
    [Table("product_tax")]
    public class ProductTax
    {
        [Key, Column("id_product", Order = 0)]
        public int IdProduct { get; set; }

        [Key, Column("id_tax", Order = 1)]
        public int IdTax { get; set; }

        // propiedad de navegacion
        [ForeignKey(nameof(IdProduct))]
        public virtual Product? Product { get; set; }

        [ForeignKey(nameof(IdTax))]
        public virtual Tax? Tax { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InvoicesSystem.API.Models.Entities
{
    [Table("payment_method")]
    public class PaymentMethod
    {
        [Key]
        [Column("id_payment_method")]
        public int IdPaymentMethod { get; set; }
        
        [Required]
        [Column("method_name")]
        [StringLength(60)]
        public string? MethodName { get; set; }

        [Column("description")]
        [StringLength(150)]
        public string? Description { get; set; }
        
        // propiedad de navegacion
        public virtual ICollection<InvoicePayment> InvoicePayments { get; set; } = new List<InvoicePayment>();
    }
}
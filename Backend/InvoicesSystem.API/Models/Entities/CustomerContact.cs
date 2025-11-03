using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using InvoicesSystem.API.Models.Enums;

namespace InvoicesSystem.API.Models.Entities
{
    [Table("customer_contact")]
    public class CustomerContact
    {
        [Key]
        [Column("id_customer_contact")]
        public int IdCustomerContact { get; set; }

        [Required]
        [Column("id_customer")]
        [ForeignKey(nameof(Customer))]
        public int IdCustomer { get; set; }

        [Required]
        [Column("contact_type")]
        public ContactType ContactType { get; set; }

        [Required]
        [StringLength(100)]
        [Column("contact_value")]
        public string? ContactValue { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        // propiedad de navegación
        public virtual Customer? Customer { get; set; }
    }
}
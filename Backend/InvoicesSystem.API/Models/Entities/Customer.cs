using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using InvoicesSystem.API.Models.Enums;


namespace InvoicesSystem.API.Models.Entities
{
    [Table("customer")]
    public class Customer
    {
        [Key]
        [Column("id_customer")]
        public int IdCustomer { get; set; }

        [Required]
        [Column("id_address")]
        [ForeignKey(nameof(Address))]
        public int IdAddress { get; set; }

        [Required]
        [Column("id_type_identification")]
        [ForeignKey(nameof(TypeIdentification))]
        public int IdTypeIdentification { get; set; }

        [Required]
        [Column("id_tax_regime")]
        [ForeignKey(nameof(TaxRegime))]
        public int IdTaxRegime { get; set; }

        [Required]
        [Column("id_tax_responsibility")]
        [ForeignKey(nameof(TaxResponsibility))]
        public int IdTaxResponsibility { get; set; }

        [Required]
        [Column("identification_number")]
        [StringLength(30)]
        public string? IdentificationNumber { get; set; }

        [Required]
        [Column("person_type")]
        public PersonType PersonType { get; set; }

        [StringLength(100)]
        [Column("first_name")]
        public string? FirstName { get; set; }

        [StringLength(100)]
        [Column("last_name")]
        public string? LastName { get; set; }

        [StringLength(200)]
        [Column("business_name")]
        public string? BusinessName { get; set; }

        [StringLength(100)]
        [Column("commercial_name")]
        public string? CommercialName { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        // propiedades de navegacion
        public virtual Address? Address { get; set; }
        public virtual TypeIdentification? TypeIdentification { get; set; }
        public virtual TaxRegime? TaxRegime { get; set; }
        public virtual TaxResponsibility? TaxResponsibility { get; set; }

        public virtual ICollection<CustomerContact> CustomerContacts { get; set; } = new List<CustomerContact>();
        public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
    }
}
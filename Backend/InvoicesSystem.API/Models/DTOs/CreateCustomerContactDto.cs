using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using InvoicesSystem.API.Models.Enums;

namespace InvoicesSystem.API.Models.DTOs
{
    public class CreateCustomerContactDto
    {
        [Required]
        public ContactType ContactType { get; set; } // tipo de contacto

        [Required]
        [StringLength(100)]
        public string? ContactValue { get; set; } // valor del contacto
    }   
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InvoicesSystem.API.Models.Enums;

namespace InvoicesSystem.API.Models.DTOs
{
    public class CustomerContactDto
    {
        public int IdCustomerContact { get; set; } // id del contacto
        public ContactType ContactType { get; set; } // tipo de contacto
        public string? ContactValue { get; set; } // valor del contacto
    }
}
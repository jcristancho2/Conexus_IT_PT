using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using InvoicesSystem.API.Models.Enums;

namespace InvoicesSystem.API.Models.DTOs;

public class CustomerDto
{
    public int IdCustomer { get; set; } // id del cliente
    public string? IdentificationNumber { get; set; } // numero de identificacion
    public PersonType PersonType { get; set; } // tipo de persona
    public string? FirstName { get; set; } // nombres
    public string? LastName { get; set; } // apellidos
    public string? BusinessName { get; set; } // razon social
    public string? CommercialName { get; set; } // nombre comercial

    // Dirección
    public string? FullAddress { get; set; } // dirección completa
    public string? CityName { get; set; } // ciudad
    public string? DepartmentName { get; set; } // departamento
    public string? CountryName { get; set; } // país

    // Información tributaria
    public string? TaxRegimeCode { get; set; } // régimen tributario
    public string? TaxResponsibilityCode { get; set; } // responsabilidad tributaria

    // Contactos
    public List<CustomerContactDto> Contacts { get; set; } = new(); // lista de contactos

    public DateTime CreatedAt { get; set; } // fecha de creación
}

public class CustomerContactDto
{
    public int IdCustomerContact { get; set; } // id del contacto
    public ContactType ContactType { get; set; } // tipo de contacto
    public string? ContactValue { get; set; } // valor del contacto
}
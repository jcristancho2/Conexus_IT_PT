using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using InvoicesSystem.API.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace InvoicesSystem.API.Models.DTOs;

public class CreateCustomerDto
{
    [Required]
    public int IdTypeIdentification { get; set; } // id del tipo de identificacion
    
    [Required]
    [StringLength(30)]
    public string? IdentificationNumber { get; set; } // numero de identificacion

    [Required]
    public PersonType PersonType { get; set; } // tipo de persona

    [StringLength(100)]
    public string? FirstName { get; set; } // nombres
    
    [StringLength(100)]
    public string? LastName { get; set; } // apellidos

    [StringLength(200)]
    public string? BusinessName { get; set; } // razon social

    [StringLength(100)]
    public string? CommercialName { get; set; } // nombre comercial
    
    [Required]
    public int IdAddress { get; set; } // id de la direccion
    
    [Required]
    public int IdTaxRegime { get; set; } // id del regimen tributario
    
    [Required]
    public int IdTaxResponsibility { get; set; } // id de la responsabilidad tributaria
    
    public List<CreateCustomerContactDto> Contacts { get; set; } = new(); // lista de contactos
}

public class CreateCustomerContactDto
{
    [Required]
    public ContactType ContactType { get; set; } // tipo de contacto
    
    [Required]
    [StringLength(100)]
    public string? ContactValue { get; set; } // valor del contacto
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using InvoicesSystem.API.Models.Enums;

namespace InvoicesSystem.API.Models.DTOs;

public class UpdateCustomerDto
{
    [Required]
    public int IdTypeIdentification { get; set; }
    
    [Required]
    [StringLength(30)]
    public string? IdentificationNumber { get; set; }

    [Required]
    public PersonType PersonType { get; set; }

    [StringLength(100)]
    public string? FirstName { get; set; }
    
    [StringLength(100)]
    public string? LastName { get; set; }

    [StringLength(200)]
    public string? BusinessName { get; set; }

    [StringLength(100)]
    public string? CommercialName { get; set; }

    [Required]
    public int IdTaxRegime { get; set; }

    [Required]
    public int IdTaxResponsibility { get; set; }
}
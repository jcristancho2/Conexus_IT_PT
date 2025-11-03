using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using InvoicesSystem.API.Models.Enums;

namespace InvoicesSystem.API.Models.DTOs;

public class RegisterRequest
{
    [Required(ErrorMessage = "El email es requerido")]
    [EmailAddress(ErrorMessage = "Email inválido")]
    [StringLength(100)]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "La contraseña es requerida")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Confirmar contraseña es requerido")]
    [Compare("Password", ErrorMessage = "Las contraseñas no coinciden")]
    public string ConfirmPassword { get; set; } = string.Empty;

    // Datos del cliente
    [Required]
    public int IdTypeIdentification { get; set; }

    [Required]
    [StringLength(50)]
    public string IdentificationNumber { get; set; } = string.Empty;

    [Required]
    public PersonType PersonType { get; set; }

    // Para Persona Natural
    [StringLength(100)]
    public string? FirstName { get; set; }

    [StringLength(100)]
    public string? LastName { get; set; }

    // Para Persona Jurídica
    [StringLength(200)]
    public string? BusinessName { get; set; }

    [StringLength(100)]
    public string? CommercialName { get; set; }

    // Dirección
    [Required]
    public int IdCity { get; set; }

    [Required]
    [StringLength(200)]
    public string FullAddress { get; set; } = string.Empty;

    // Régimen tributario
    public int? IdTaxRegime { get; set; }
    public int? IdTaxResponsibility { get; set; }

    // Contactos adicionales (opcional)
    public List<CustomerContactDto>? Contacts { get; set; }
}
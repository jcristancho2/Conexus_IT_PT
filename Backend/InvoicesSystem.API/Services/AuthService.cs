using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;
using InvoicesSystem.API.Models.DTOs;
using InvoicesSystem.API.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using InvoicesSystem.API.Persistence;
using InvoicesSystem.API.Models.Enums;
using InvoicesSystem.API.Services.Interfaces;
using System.Text;

namespace InvoicesSystem.API.Services;

public class AuthService : IAuthService
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;

    public AuthService(AppDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task<LoginResponse?> LoginAsync(string email, string password)
    {
        var customer = await _context.Customers
            .Include(c => c.CustomerContacts)
            .Include(c => c.Address)
                .ThenInclude(a => a!.City)
                    .ThenInclude(c => c!.Department)
                        .ThenInclude(d => d!.Country)
            .Include(c => c.TypeIdentification)
            .Include(c => c.TaxRegime)
            .Include(c => c.TaxResponsibility)
            .FirstOrDefaultAsync(c => c.Email == email);

        if (customer == null)
            return null;

        // Comparar password directamente (sin hash por ahora)
        if (customer.PasswordHash != password)
            return null;

        var token = GenerateJwtToken(customer);

        // ✅ LÍNEA 53 CORREGIDA - Usar GetValue en lugar de int.Parse
        var expirationMinutes = _configuration.GetValue<int>("Jwt:ExpirationInMinutes", 1440);

        return new LoginResponse
        {
            Token = token,
            Expiration = DateTime.UtcNow.AddMinutes(expirationMinutes),
            Customer = MapToDto(customer)
        };
    }

    public async Task<LoginResponse?> RegisterAsync(RegisterRequest request)
    {
        // Verificar si el usuario ya existe
        var existingCustomer = await _context.Customers
            .FirstOrDefaultAsync(c => c.Email == request.Email);

        if (existingCustomer != null)
            return null;

        // Crear dirección
        var address = new Address
        {
            FullAddress = request.FullAddress,
            IdCity = request.IdCity
        };

        _context.Addresses.Add(address);
        await _context.SaveChangesAsync();

        // Crear cliente
        var customer = new Customer
        {
            IdTypeIdentification = request.IdTypeIdentification,
            IdentificationNumber = request.IdentificationNumber,
            PersonType = request.PersonType,
            FirstName = request.FirstName,
            LastName = request.LastName,
            BusinessName = request.BusinessName,
            CommercialName = request.CommercialName,
            IdAddress = address.IdAddress,
            IdTaxRegime = request.IdTaxRegime,
            IdTaxResponsibility = request.IdTaxResponsibility,
            Email = request.Email,
            PasswordHash = request.Password, // Sin hash por ahora
            CreatedAt = DateTime.UtcNow
        };

        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();

        // Recargar con relaciones
        customer = await _context.Customers
            .Include(c => c.CustomerContacts)
            .Include(c => c.Address)
                .ThenInclude(a => a!.City)
                    .ThenInclude(c => c!.Department)
                        .ThenInclude(d => d!.Country)
            .Include(c => c.TypeIdentification)
            .Include(c => c.TaxRegime)
            .Include(c => c.TaxResponsibility)
            .FirstAsync(c => c.IdCustomer == customer.IdCustomer);

        var token = GenerateJwtToken(customer);
        
        var expirationMinutes = _configuration.GetValue<int>("Jwt:ExpirationInMinutes", 1440);

        return new LoginResponse
        {
            Token = token,
            Expiration = DateTime.UtcNow.AddMinutes(expirationMinutes),
            Customer = MapToDto(customer)
        };
    }

    public async Task<Customer?> GetUserByEmailAsync(string email)
    {
        return await _context.Customers
            .Include(c => c.CustomerContacts)
            .FirstOrDefaultAsync(c => c.Email == email);
    }

    public string GenerateJwtToken(Customer customer)
    {
        var jwtKey = _configuration["Jwt:Key"];
        var issuer = _configuration["Jwt:Issuer"] ?? "InvoicesSystem.API";
        var audience = _configuration["Jwt:Audience"] ?? "InvoicesSystem.Users";

        if (string.IsNullOrEmpty(jwtKey))
        {
            throw new InvalidOperationException("JWT Key no está configurado en appsettings.json");
        }

        var key = Encoding.UTF8.GetBytes(jwtKey);
        
        var expirationMinutes = _configuration.GetValue<int>("Jwt:ExpirationInMinutes", 1440);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, customer.IdCustomer.ToString()),
            new Claim(ClaimTypes.Email, customer.Email),
            new Claim(ClaimTypes.Name, $"{customer.FirstName} {customer.LastName}".Trim()),
            new Claim("PersonType", customer.PersonType.ToString())
        };

        var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expirationMinutes),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private CustomerDto MapToDto(Customer customer)
    {
        return new CustomerDto
        {
            IdCustomer = customer.IdCustomer,
            IdentificationNumber = customer.IdentificationNumber,
            PersonType = customer.PersonType,
            FirstName = customer.FirstName,
            LastName = customer.LastName,
            BusinessName = customer.BusinessName,
            CommercialName = customer.CommercialName,
            FullAddress = customer.Address?.FullAddress,
            CityName = customer.Address?.City?.NameCity,
            DepartmentName = customer.Address?.City?.Department?.NameDepartment,
            CountryName = customer.Address?.City?.Department?.Country?.Name_country,
            TaxRegimeCode = customer.TaxRegime?.Code,
            TaxResponsibilityCode = customer.TaxResponsibility?.Code,
            Contacts = customer.CustomerContacts.Select(cc => new CustomerContactDto
            {
                IdCustomerContact = cc.IdCustomerContact,
                ContactType = cc.ContactType,
                ContactValue = cc.ContactValue
            }).ToList(),
            CreatedAt = customer.CreatedAt
        };
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InvoicesSystem.API.Data;
using InvoicesSystem.API.Models.DTOs;
using InvoicesSystem.API.Models.Entities;
using InvoicesSystem.API.Models.Enums;

namespace InvoicesSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly AppDbContext _context;

    public CustomersController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/Customers
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CustomerDto>>> GetCustomers(
        [FromQuery] string? search = null,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        var query = _context.Customers
            .Include(c => c.Address)
                .ThenInclude(a => a.City)
                    .ThenInclude(c => c.Departament)
                        .ThenInclude(d => d.Country)
            .Include(c => c.TaxRegime)
            .Include(c => c.TaxResponsibility)
            .Include(c => c.CustomerContacts)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(c =>
                c.IdentificationNumber.Contains(search) ||
                (c.FirstName != null && c.FirstName.Contains(search)) ||
                (c.LastName != null && c.LastName.Contains(search)) ||
                (c.BusinessName != null && c.BusinessName.Contains(search)) ||
                (c.CommercialName != null && c.CommercialName.Contains(search))
            );
        }

        var total = await query.CountAsync();
        
        var customers = await query
            .OrderBy(c => c.IdCustomer)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var result = customers.Select(c => MapToDto(c)).ToList();

        Response.Headers.Add("X-Total-Count", total.ToString());
        Response.Headers.Add("X-Page", page.ToString());
        Response.Headers.Add("X-Page-Size", pageSize.ToString());

        return Ok(result);
    }

    // GET: api/Customers/5
    [HttpGet("{id}")]
    public async Task<ActionResult<CustomerDto>> GetCustomer(int id)
    {
        var customer = await _context.Customers
            .Include(c => c.Address)
                .ThenInclude(a => a.City)
                    .ThenInclude(c => c.Departament)
                        .ThenInclude(d => d.Country)
            .Include(c => c.TaxRegime)
            .Include(c => c.TaxResponsibility)
            .Include(c => c.CustomerContacts)
            .FirstOrDefaultAsync(c => c.IdCustomer == id);

        if (customer == null)
            return NotFound(new { message = $"Cliente con ID {id} no encontrado" });

        return Ok(MapToDto(customer));
    }

    // POST: api/Customers
    [HttpPost]
    public async Task<ActionResult<CustomerDto>> CreateCustomer(CreateCustomerDto dto)
    {
        // Validar que no exista un cliente con la misma identificación
        var existingCustomer = await _context.Customers
            .FirstOrDefaultAsync(c => c.IdentificationNumber == dto.IdentificationNumber);
        
        if (existingCustomer != null)
            return BadRequest(new { message = "Ya existe un cliente con ese número de identificación" });

        // Validar dirección
        var address = await _context.Addresses.FindAsync(dto.IdAddress);
        if (address == null)
            return BadRequest(new { message = "Dirección no encontrada" });

        var customer = new Customer
        {
            IdTypeIdentification = dto.IdTypeIdentification,
            IdentificationNumber = dto.IdentificationNumber,
            PersonType = dto.PersonType,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            BusinessName = dto.BusinessName,
            CommercialName = dto.CommercialName,
            IdAddress = dto.IdAddress,
            IdTaxRegime = dto.IdTaxRegime,
            IdTaxResponsibility = dto.IdTaxResponsibility,
            CreatedAt = DateTime.UtcNow
        };

        // Agregar contactos
        foreach (var contactDto in dto.Contacts)
        {
            customer.CustomerContacts.Add(new CustomerContact
            {
                ContactType = contactDto.ContactType,
                ContactValue = contactDto.ContactValue,
                CreatedAt = DateTime.UtcNow
            });
        }

        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();

        // Recargar con todas las relaciones
        var createdCustomer = await _context.Customers
            .Include(c => c.Address)
                .ThenInclude(a => a.City)
                    .ThenInclude(c => c.Departament)
                        .ThenInclude(d => d.Country)
            .Include(c => c.TaxRegime)
            .Include(c => c.TaxResponsibility)
            .Include(c => c.CustomerContacts)
            .FirstAsync(c => c.IdCustomer == customer.IdCustomer);

        return CreatedAtAction(nameof(GetCustomer), new { id = customer.IdCustomer }, MapToDto(createdCustomer));
    }

    // PUT: api/Customers/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCustomer(int id, CreateCustomerDto dto)
    {
        var customer = await _context.Customers
            .Include(c => c.CustomerContacts)
            .FirstOrDefaultAsync(c => c.IdCustomer == id);
        
        if (customer == null)
            return NotFound(new { message = $"Cliente con ID {id} no encontrado" });

        customer.IdTypeIdentification = dto.IdTypeIdentification;
        customer.IdentificationNumber = dto.IdentificationNumber;
        customer.PersonType = dto.PersonType;
        customer.FirstName = dto.FirstName;
        customer.LastName = dto.LastName;
        customer.BusinessName = dto.BusinessName;
        customer.CommercialName = dto.CommercialName;
        customer.IdAddress = dto.IdAddress;
        customer.IdTaxRegime = dto.IdTaxRegime;
        customer.IdTaxResponsibility = dto.IdTaxResponsibility;
        customer.UpdatedAt = DateTime.UtcNow;

        // Actualizar contactos
        _context.CustomerContacts.RemoveRange(customer.CustomerContacts);
        
        foreach (var contactDto in dto.Contacts)
        {
            customer.CustomerContacts.Add(new CustomerContact
            {
                ContactType = contactDto.ContactType,
                ContactValue = contactDto.ContactValue,
                CreatedAt = DateTime.UtcNow
            });
        }

        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/Customers/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCustomer(int id)
    {
        var customer = await _context.Customers.FindAsync(id);
        
        if (customer == null)
            return NotFound(new { message = $"Cliente con ID {id} no encontrado" });

        // Verificar si tiene facturas
        var hasInvoices = await _context.Invoices.AnyAsync(i => i.IdCustomer == id);
        if (hasInvoices)
            return BadRequest(new { message = "No se puede eliminar el cliente porque tiene facturas asociadas" });

        _context.Customers.Remove(customer);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private CustomerDto MapToDto(Customer customer)
    {
        return new CustomerDto
        {
            IdCustomer = customer.IdCustomer,
            IdentificationNumber = customer.IdentificationNumber ?? "",
            PersonType = customer.PersonType,
            FirstName = customer.FirstName,
            LastName = customer.LastName,
            BusinessName = customer.BusinessName,
            CommercialName = customer.CommercialName,
            FullAddress = customer.Address?.FullAddress ?? "",
            CityName = customer.Address?.City?.NameCity ?? "",
            DepartmentName = customer.Address?.City?.Departament?.NameDepartment ?? "",
            CountryName = customer.Address?.City?.Departament?.Country?.Name_country ?? "",
            TaxRegimeCode = customer.TaxRegime?.Code ?? "",
            TaxResponsibilityCode = customer.TaxResponsibility?.Code ?? "",
            Contacts = customer.CustomerContacts?.Select(c => new CustomerContactDto
            {
                IdCustomerContact = c.IdCustomerContact,
                ContactType = c.ContactType,
                ContactValue = c.ContactValue ?? ""
            }).ToList() ?? new List<CustomerContactDto>(),
            CreatedAt = customer.CreatedAt
        };
    }
}
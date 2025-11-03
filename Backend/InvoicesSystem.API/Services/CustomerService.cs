using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using InvoicesSystem.API.Models.DTOs;
using InvoicesSystem.API.Models.Entities;
using InvoicesSystem.API.Repositories.Interfaces;
using InvoicesSystem.API.Services.Interfaces;
using InvoicesSystem.API.Persistence;
using Microsoft.EntityFrameworkCore;

namespace InvoicesSystem.API.Services;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;
    private readonly AppDbContext _context;

    public CustomerService(ICustomerRepository customerRepository, IMapper mapper, AppDbContext context)
    {
        _customerRepository = customerRepository;
        _mapper = mapper;
        _context = context;
    }

    public async Task<(IEnumerable<CustomerDto> customers, int total)> GetAllAsync(int page = 1, int pageSize = 10, string? search = null)
    {
        var (customers, total) = await _customerRepository.GetPagedAsync(page, pageSize, search);
        var customerDtos = _mapper.Map<IEnumerable<CustomerDto>>(customers);
        return (customerDtos, total);
    }

    public async Task<CustomerDto?> GetByIdAsync(int id)
    {
        var customer = await _customerRepository.GetByIdWithDetailsAsync(id);
        return customer == null ? null : _mapper.Map<CustomerDto>(customer);
    }

    public async Task<CustomerDto> CreateAsync(CreateCustomerDto dto)
    {
        // Validar duplicados
        var exists = await _customerRepository.ExistsByIdentificationAsync(dto.IdentificationNumber!);
        if (exists)
            throw new InvalidOperationException("Ya existe un cliente con ese número de identificación");

        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            // 1. Crear y guardar la dirección primero
            var address = new Address
            {
                FullAddress = dto.FullAddress,
                IdCity = dto.IdCity
            };

            _context.Addresses.Add(address);
            await _context.SaveChangesAsync();

            // 2. Crear el customer con la dirección
            var customer = new Customer
            {
                IdTypeIdentification = dto.IdTypeIdentification,
                IdentificationNumber = dto.IdentificationNumber!,
                PersonType = dto.PersonType,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                BusinessName = dto.BusinessName,
                CommercialName = dto.CommercialName,
                IdAddress = address.IdAddress,
                IdTaxRegime = dto.IdTaxRegime,
                IdTaxResponsibility = dto.IdTaxResponsibility,
                CreatedAt = DateTime.UtcNow
            };

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            // 3. Crear los contactos si existen
            if (dto.Contacts != null && dto.Contacts.Any())
            {
                var contacts = dto.Contacts.Select(c => new CustomerContact
                {
                    IdCustomer = customer.IdCustomer,
                    ContactType = c.ContactType,
                    ContactValue = c.ContactValue
                }).ToList();

                _context.CustomerContacts.AddRange(contacts);
                await _context.SaveChangesAsync();
            }

            await transaction.CommitAsync();

            // 4. Recargar con todas las relaciones
            var createdCustomer = await _context.Customers
                .Include(c => c.Address)
                    .ThenInclude(a => a!.City)
                        .ThenInclude(city => city!.Department)
                            .ThenInclude(dept => dept!.Country)
                .Include(c => c.TypeIdentification)
                .Include(c => c.TaxRegime)
                .Include(c => c.TaxResponsibility)
                .Include(c => c.CustomerContacts)
                .FirstAsync(c => c.IdCustomer == customer.IdCustomer);

            return _mapper.Map<CustomerDto>(createdCustomer);
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<CustomerDto?> UpdateAsync(int id, UpdateCustomerDto dto)
    {
        var customer = await _customerRepository.GetByIdAsync(id);
        if (customer == null)
            return null;

        // Validar duplicados excluyendo el actual
        var exists = await _customerRepository.ExistsByIdentificationAsync(dto.IdentificationNumber!, id);
        if (exists)
            throw new InvalidOperationException("Ya existe otro cliente con ese número de identificación");

        _mapper.Map(dto, customer);
        customer.UpdatedAt = DateTime.UtcNow;

        await _customerRepository.UpdateAsync(customer);
        return _mapper.Map<CustomerDto>(customer);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var customer = await _customerRepository.GetByIdAsync(id);
        if (customer == null)
            return false;

        await _customerRepository.DeleteAsync(customer);
        return true;
    }
}
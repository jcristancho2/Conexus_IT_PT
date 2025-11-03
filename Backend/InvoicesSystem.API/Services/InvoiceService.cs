using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using InvoicesSystem.API.Models.DTOs;
using InvoicesSystem.API.Models.Entities;
using InvoicesSystem.API.Models.Enums;
using InvoicesSystem.API.Repositories.Interfaces;
using InvoicesSystem.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InvoicesSystem.API.Services;

public class InvoiceService : IInvoiceService
{
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public InvoiceService(
        IInvoiceRepository invoiceRepository,
        ICustomerRepository customerRepository,
        IProductRepository productRepository,
        IMapper mapper)
    {
        _invoiceRepository = invoiceRepository;
        _customerRepository = customerRepository;
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public async Task<InvoiceDto> CreateAsync(CreateInvoiceDto dto)
    {
        try
        {
            // Validar que el customer existe
            var customer = await _customerRepository.GetByIdAsync(dto.IdCustomer);
            if (customer == null)
                throw new ArgumentException($"Customer con ID {dto.IdCustomer} no existe");

            // Validar que los productos existen
            if (dto.Details != null && dto.Details.Any())
            {
                foreach (var detail in dto.Details)
                {
                    var product = await _productRepository.GetByIdAsync(detail.IdProduct);
                    if (product == null)
                        throw new ArgumentException($"Product con ID {detail.IdProduct} no existe");
                }
            }

            // Generar número de factura
            var invoiceNumber = await _invoiceRepository.GenerateInvoiceNumberAsync();

            // Crear la factura usando las propiedades correctas
            var invoice = new Invoice
            {
                InvoiceNumber = invoiceNumber,
                IdCustomer = dto.IdCustomer,
                // IdIssuer = 1, // ← COMENTAR ESTA LÍNEA TEMPORALMENTE
                InvoiceDate = DateTime.UtcNow.Date,
                DueDate = dto.DueDate?.Date,
                Status = InvoiceStatus.Draft,
                Subtotal = dto.SubtotalAmount,
                TotalTax = dto.TaxAmount,
                Total = dto.TotalAmount,
                Notes = dto.Notes,
                CreatedAt = DateTime.UtcNow,
                InvoiceDetails = new List<InvoiceDetail>(),
                InvoicePayments = new List<InvoicePayment>()
            };

            // Agregar detalles si existen
            if (dto.Details != null && dto.Details.Any())
            {
                foreach (var detailDto in dto.Details)
                {
                    var detail = new InvoiceDetail
                    {
                        IdProduct = detailDto.IdProduct,
                        Quantity = detailDto.Quantity,
                        UnitPrice = detailDto.UnitPrice,
                        Discount = detailDto.DiscountAmount,
                        Subtotal = detailDto.TotalAmount, // Mapear TotalAmount a Subtotal
                        Description = "", // Agregar si está disponible en el DTO
                        CreatedAt = DateTime.UtcNow,
                        InvoiceDetailTaxes = new List<InvoiceDetailTax>()
                    };
                    
                    invoice.InvoiceDetails.Add(detail);
                }
            }

            // Guardar en base de datos
            var createdInvoice = await _invoiceRepository.AddAsyncCustom(invoice);
            Console.WriteLine($"Invoice guardada en DB con ID: {createdInvoice.IdInvoice}");

            // Obtener la factura completa con todas las relaciones
            var invoiceWithDetails = await _invoiceRepository.GetByIdWithDetailsAsync(createdInvoice.IdInvoice);
            Console.WriteLine($"Invoice obtenida de DB: {invoiceWithDetails != null}");

            if (invoiceWithDetails == null)
            {
                Console.WriteLine($"ERROR: No se pudo obtener la factura con ID {createdInvoice.IdInvoice}");
                throw new InvalidOperationException($"No se pudo obtener la factura creada con ID {createdInvoice.IdInvoice}");
            }

            // Mapear a DTO
            var result = _mapper.Map<InvoiceDto>(invoiceWithDetails);
            Console.WriteLine($"Mapping completado: {result != null}");

            if (result != null)
            {
                Console.WriteLine($"Result ID: {result.IdInvoice}");
            }

            Console.WriteLine($"CreateAsync completado exitosamente");
            return result ?? throw new InvalidOperationException("Error en el mapeo de la factura");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error en CreateAsync: {ex.Message}");
            Console.WriteLine($"StackTrace: {ex.StackTrace}");
            throw;
        }
    }

    public async Task<InvoiceDto?> GetByIdAsync(int id)
    {
        var invoice = await _invoiceRepository.GetByIdWithDetailsAsync(id);
        return invoice != null ? _mapper.Map<InvoiceDto>(invoice) : null;
    }

    public async Task<(IEnumerable<InvoiceDto> invoices, int total)> GetAllAsync(int page = 1, int pageSize = 10, string? search = null)
    {
        var (invoices, total) = await _invoiceRepository.GetPagedAsync(page, pageSize, search);
        var invoiceDtos = _mapper.Map<IEnumerable<InvoiceDto>>(invoices);
        return (invoiceDtos, total);
    }

    public async Task<InvoiceDto?> UpdateAsync(int id, UpdateInvoiceDto dto)
    {
        var invoice = await _invoiceRepository.GetByIdAsync(id);
        if (invoice == null)
            return null;

        _mapper.Map(dto, invoice);
        invoice.UpdatedAt = DateTime.UtcNow;

        await _invoiceRepository.UpdateAsync(invoice);
        await _invoiceRepository.SaveChangesAsync();
        var updatedInvoice = await _invoiceRepository.GetByIdWithDetailsAsync(id);
        
        return _mapper.Map<InvoiceDto>(updatedInvoice);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var invoice = await _invoiceRepository.GetByIdAsync(id);
        if (invoice == null)
            return false;

        await _invoiceRepository.DeleteAsync(invoice);
        await _invoiceRepository.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<InvoiceDto>> GetInvoicesByCustomerAsync(int customerId)
    {
        var invoices = await _invoiceRepository.GetByCustomerIdAsync(customerId);
        return _mapper.Map<IEnumerable<InvoiceDto>>(invoices);
    }

    public async Task<IEnumerable<InvoiceDto>> GetInvoicesByStatusAsync(InvoiceStatus status)
    {
        var invoices = await _invoiceRepository.GetByStatusAsync(status);
        return _mapper.Map<IEnumerable<InvoiceDto>>(invoices);
    }

    public async Task<IEnumerable<InvoiceDto>> GetInvoicesByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        var invoices = await _invoiceRepository.GetByDateRangeAsync(startDate, endDate);
        return _mapper.Map<IEnumerable<InvoiceDto>>(invoices);
    }
}
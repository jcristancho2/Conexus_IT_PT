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
public class InvoicesController : ControllerBase
{
    private readonly AppDbContext _context; // Inyección de dependencia del DbContext

    public InvoicesController(AppDbContext context) // Constructor
    {
        _context = context; // Asignación del DbContext inyectado
    }

    // GET: api/invoices
    [HttpGet]
    public async Task<ActionResult<IEnumerable<InvoiceDto>>> GetInvoices(
        [FromQuery] InvoiceStatus? status = null, // Filtro por estado
        [FromQuery] DateTime? dateFrom = null, // Filtro por fecha desde
        [FromQuery] DateTime? dateTo = null, // Filtro por fecha hasta
        [FromQuery] int page = 1, // Filtro por página
        [FromQuery] int pageSize = 10) // Filtro por tamaño de página
    {
        var query = _context.Invoices
            .Include(i => i.Customer)
            .Include(i => i.Issuer)
            .Include(i => i.InvoiceDetails)
                .ThenInclude(d => d.Product)
            .Include(i => i.InvoiceDetails)
                .ThenInclude(d => d.InvoiceDetailTaxes)
                    .ThenInclude(t => t.Tax)
            .Include(i => i.InvoicePayments)
                .ThenInclude(p => p.PaymentMethod)
            .AsQueryable();

        // Filtros
        if (status.HasValue)
            query = query.Where(i => i.Status == status.Value); // Filtrar por estado

        if (dateFrom.HasValue)
            query = query.Where(i => i.InvoiceDate >= dateFrom.Value); // Filtrar por fecha desde

        if (dateTo.HasValue)
            query = query.Where(i => i.InvoiceDate <= dateTo.Value); // Filtrar por fecha hasta

        var total = await query.CountAsync(); // Conteo total para paginación

        var invoices = await query
            .OrderByDescending(i => i.InvoiceDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize) 
            .ToListAsync(); // Paginación

        var result = invoices.Select(i => MapToDto(i)).ToList(); // Mapeo a DTO

        Response.Headers.Add("X-Total-Count", total.ToString()); // Conteo total
        Response.Headers.Add("X-Page", page.ToString()); // Página actual
        Response.Headers.Add("X-Page-Size", pageSize.ToString()); // Tamaño de página

        return Ok(result);
    }

// GET: api/Invoices/5
    [HttpGet("{id}")]
    public async Task<ActionResult<InvoiceDto>> GetInvoice(int id) // Obtener factura por ID
    {
        var invoice = await _context.Invoices
            .Include(i => i.Customer)
                .ThenInclude(c => c.Address)
                    .ThenInclude(a => a.City)
                        .ThenInclude(c => c.Departament)
                            .ThenInclude(d => d.Country)
            .Include(i => i.Issuer)
            .Include(i => i.InvoiceDetails)
                .ThenInclude(d => d.Product)
            .Include(i => i.InvoiceDetails)
                .ThenInclude(d => d.InvoiceDetailTaxes)
                    .ThenInclude(t => t.Tax)
            .Include(i => i.InvoicePayments)
                .ThenInclude(p => p.PaymentMethod)
            .FirstOrDefaultAsync(i => i.IdInvoice == id); // Buscar factura por ID

        if (invoice == null)
            return NotFound(new { message = $"Factura con ID {id} no encontrada" });

        return Ok(MapToDto(invoice)); // Mapeo a DTO y retorno
    }

    // POST: api/Invoices
    [HttpPost]
    public async Task<ActionResult<InvoiceDto>> CreateInvoice(CreateInvoiceDto dto) // Crear nueva factura
    {
        var customer = await _context.Customers.FindAsync(dto.IdCustomer);
        if (customer == null)
            return BadRequest(new { message = "Cliente no encontrado" }); // Validar existencia de cliente

        var issuer = await _context.Issuers.FindAsync(dto.IdIssuer);
        if (issuer == null)
            return BadRequest(new { message = "Emisor no encontrado" }); // Validar existencia de emisor

        var existingInvoice = await _context.Invoices
            .FirstOrDefaultAsync(i => i.InvoiceNumber == dto.InvoiceNumber);
        if (existingInvoice != null)
            return BadRequest(new { message = "El número de factura ya existe" }); // Validar número de factura único

        // Crear la factura
        var invoice = new Invoice
        {
            IdCustomer = dto.IdCustomer,
            IdIssuer = dto.IdIssuer,
            InvoiceNumber = dto.InvoiceNumber,
            InvoiceDate = dto.InvoiceDate,
            DueDate = dto.DueDate,
            Status = InvoiceStatus.Draft,
            Notes = dto.Notes,
            CreatedAt = DateTime.UtcNow
        };

        decimal subtotal = 0;
        decimal totalTax = 0;

        // Procesar detalles
        foreach (var detailDto in dto.Details)
        {
            var product = await _context.Products
                .Include(p => p.ProductTaxes)
                    .ThenInclude(pt => pt.Tax)
                .FirstOrDefaultAsync(p => p.IdProduct == detailDto.IdProduct);

            if (product == null)
                return BadRequest(new { message = $"Producto con ID {detailDto.IdProduct} no encontrado" });

            var detailSubtotal = (detailDto.Quantity * detailDto.UnitPrice) - detailDto.Discount;
            subtotal += detailSubtotal;

            var detail = new InvoiceDetail
            {
                IdProduct = detailDto.IdProduct,
                Quantity = detailDto.Quantity,
                UnitPrice = detailDto.UnitPrice,
                Discount = detailDto.Discount,
                Subtotal = detailSubtotal,
                Description = detailDto.Description,
                CreatedAt = DateTime.UtcNow
            };

            // Calcular impuestos
            var taxIds = detailDto.TaxIds.Any() ? detailDto.TaxIds : product.ProductTaxes.Select(pt => pt.IdTax).ToList();
            
            foreach (var taxId in taxIds)
            {
                var tax = await _context.Taxes.FindAsync(taxId);
                if (tax == null) continue;

                var taxAmount = detailSubtotal * tax.TaxRate;
                totalTax += taxAmount;

                detail.InvoiceDetailTaxes.Add(new InvoiceDetailTax
                {
                    IdTax = taxId,
                    TaxBase = detailSubtotal,
                    TaxAmount = taxAmount,
                    CreatedAt = DateTime.UtcNow
                });
            }

            invoice.InvoiceDetails.Add(detail);
        }

        invoice.Subtotal = subtotal;
        invoice.TotalTax = totalTax;
        invoice.Total = subtotal + totalTax;

        // Procesar pagos
        foreach (var paymentDto in dto.Payments)
        {
            var paymentMethod = await _context.PaymentMethods.FindAsync(paymentDto.IdPaymentMethod);
            if (paymentMethod == null)
                return BadRequest(new { message = $"Método de pago con ID {paymentDto.IdPaymentMethod} no encontrado" });

            invoice.InvoicePayments.Add(new InvoicePayment
            {
                IdPaymentMethod = paymentDto.IdPaymentMethod,
                Amount = paymentDto.Amount,
                PaymentDate = paymentDto.PaymentDate,
                Reference = paymentDto.Reference,
                CreatedAt = DateTime.UtcNow
            });
        }

        _context.Invoices.Add(invoice);
        await _context.SaveChangesAsync();

        // Recargar con todas las relaciones
        var createdInvoice = await _context.Invoices
            .Include(i => i.Customer)
            .Include(i => i.Issuer)
            .Include(i => i.InvoiceDetails)
                .ThenInclude(d => d.Product)
            .Include(i => i.InvoiceDetails)
                .ThenInclude(d => d.InvoiceDetailTaxes)
                    .ThenInclude(t => t.Tax)
            .Include(i => i.InvoicePayments)
                .ThenInclude(p => p.PaymentMethod)
            .FirstAsync(i => i.IdInvoice == invoice.IdInvoice);

        return CreatedAtAction(nameof(GetInvoice), new { id = invoice.IdInvoice }, MapToDto(createdInvoice));
    }

    // PUT: api/Invoices/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateInvoice(int id, UpdateInvoiceDto dto)
    {
        var invoice = await _context.Invoices.FindAsync(id);
        
        if (invoice == null)
            return NotFound(new { message = $"Factura con ID {id} no encontrada" }); // Verificar existencia de la factura

        // Solo se puede actualizar el estado y las notas
        invoice.Status = dto.Status;
        invoice.DueDate = dto.DueDate;
        invoice.Notes = dto.Notes;
        invoice.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/Invoices/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteInvoice(int id)
    {
        var invoice = await _context.Invoices.FindAsync(id);
        
        if (invoice == null)
            return NotFound(new { message = $"Factura con ID {id} no encontrada" }); // Verificar existencia de la factura

        if (invoice.Status != InvoiceStatus.Draft)
            return BadRequest(new { message = "Solo se pueden eliminar facturas en estado borrador" }); // Validar estado

        _context.Invoices.Remove(invoice);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // Helper method para mapear a DTO
    private InvoiceDto MapToDto(Invoice invoice)
    {
        var customerName = invoice.Customer.PersonType == PersonType.Natural
            ? $"{invoice.Customer.FirstName} {invoice.Customer.LastName}"
            : invoice.Customer.BusinessName ?? invoice.Customer.CommercialName ?? "";

        return new InvoiceDto
        {
            IdInvoice = invoice.IdInvoice,
            InvoiceNumber = invoice.InvoiceNumber ?? "",
            InvoiceDate = invoice.InvoiceDate,
            DueDate = invoice.DueDate,
            Status = invoice.Status,
            Subtotal = invoice.Subtotal,
            TotalTax = invoice.TotalTax,
            Total = invoice.Total,
            Notes = invoice.Notes,
            IdCustomer = invoice.IdCustomer,
            CustomerName = customerName,
            CustomerIdentification = invoice.Customer.IdentificationNumber ?? "",
            IdIssuer = invoice.IdIssuer,
            IssuerBusinessName = invoice.Issuer.BusinessName ?? "",
            Details = invoice.InvoiceDetails?.Select(d => new InvoiceDetailDto
            {
                IdProduct = d.IdProduct,
                ProductName = d.Product?.ProductName ?? "",
                Quantity = d.Quantity,
                UnitPrice = d.UnitPrice,
                Discount = d.Discount,
                Subtotal = d.Subtotal,
                Description = d.Description,
                Taxes = d.InvoiceDetailTaxes?.Select(t => new InvoiceDetailTaxDto
                {
                    IdTax = t.IdTax,
                    TaxName = t.Tax?.TaxName ?? "",
                    TaxRate = t.Tax?.TaxRate ?? 0,
                    TaxBase = t.TaxBase,
                    TaxAmount = t.TaxAmount
                }).ToList() ?? new List<InvoiceDetailTaxDto>()
            }).ToList() ?? new List<InvoiceDetailDto>(),
            Payments = invoice.InvoicePayments?.Select(p => new InvoicePaymentDto
            {
                IdPaymentMethod = p.IdPaymentMethod,
                PaymentMethodName = p.PaymentMethod?.MethodName ?? "",
                Amount = p.Amount,
                PaymentDate = p.PaymentDate,
                Reference = p.Reference
            }).ToList() ?? new List<InvoicePaymentDto>(),
            CreatedAt = invoice.CreatedAt,
            UpdatedAt = invoice.UpdatedAt
        };
    }
}

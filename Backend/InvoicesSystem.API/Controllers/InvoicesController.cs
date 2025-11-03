using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using InvoicesSystem.API.Services.Interfaces;
using InvoicesSystem.API.Models.DTOs;
using InvoicesSystem.API.Models.Responses;

namespace InvoicesSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class InvoicesController : ControllerBase
{
    private readonly IInvoiceService _invoiceService;

    public InvoicesController(IInvoiceService invoiceService)
    {
        _invoiceService = invoiceService ?? throw new ArgumentNullException(nameof(invoiceService));
    }

    // GET: api/invoices
    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<InvoiceDto>>>> GetAll(
        [FromQuery] int page = 1, // Filtro por página
        [FromQuery] int pageSize = 10, // Filtro por tamaño de página
        [FromQuery] string? search = null) // Filtro por búsqueda
    {
        var (invoices, total) = await _invoiceService.GetAllAsync(page, pageSize, search);

        Response.Headers["X-Total-Count"] = total.ToString();
        Response.Headers["X-Page"] = page.ToString();
        Response.Headers["X-Page-Size"] = pageSize.ToString();

        return Ok(ApiResponse<IEnumerable<InvoiceDto>>.SuccessResponse(
            invoices,
            $"Se encontraron {total} facturas"
        ));
    }

// GET: api/Invoices/5
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<InvoiceDto>>> GetById(int id) // Obtener factura por ID
    {
        var invoice = await _invoiceService.GetByIdAsync(id);

        if (invoice == null)
            return NotFound(ApiResponse<InvoiceDto>.NotFoundResponse("Factura no encontrada"));

        return Ok(ApiResponse<InvoiceDto>.SuccessResponse(invoice, "Factura obtenida exitosamente"));
    }

    // POST: api/Invoices
    [HttpPost]
    public async Task<ActionResult<ApiResponse<InvoiceDto>>> Create([FromBody] CreateInvoiceDto dto)
    {
        Console.WriteLine($"=== DEBUG CREATE INVOICE ===");
        Console.WriteLine($"dto is null: {dto == null}");
        Console.WriteLine($"_invoiceService is null: {_invoiceService == null}");
        
        if (dto == null)
        {
            return BadRequest("Los datos de la factura no pueden estar vacíos");
        }

        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();
            return BadRequest(ApiResponse<InvoiceDto>.ValidationErrorResponse(errors));
        }

        try
        {
            Console.WriteLine("Llamando a _invoiceService.CreateAsync...");
            var result = await _invoiceService.CreateAsync(dto);
            Console.WriteLine("CreateAsync completado exitosamente");
            
            if (result == null)
            {
                Console.WriteLine("ERROR: CreateAsync retornó null");
                return StatusCode(500, ApiResponse<InvoiceDto>.ErrorResponse("Error: No se pudo crear la factura"));
            }
            
            Console.WriteLine($"Invoice creada con ID: {result.IdInvoice}");
            
            return CreatedAtAction(
                nameof(GetById),
                new { id = result.IdInvoice },
                ApiResponse<InvoiceDto>.SuccessResponse(result, "Factura creada exitosamente")
            );
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ApiResponse<InvoiceDto>.ErrorResponse(ex.Message));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error en Create: {ex.Message}");
            Console.WriteLine($"StackTrace: {ex.StackTrace}");
            return StatusCode(500, ApiResponse<InvoiceDto>.ErrorResponse($"Error interno: {ex.Message}"));
        }
    }

    // PUT: api/Invoices/5
    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<InvoiceDto>>> Update(int id, [FromBody] UpdateInvoiceDto dto)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();
            return BadRequest(ApiResponse<InvoiceDto>.ValidationErrorResponse(errors));
        }

        try
        {
            var invoice = await _invoiceService.UpdateAsync(id, dto);

            if (invoice == null)
                return NotFound(ApiResponse<InvoiceDto>.NotFoundResponse("Factura no encontrada"));

            return Ok(ApiResponse<InvoiceDto>.SuccessResponse(invoice, "Factura actualizada exitosamente"));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ApiResponse<InvoiceDto>.ErrorResponse(ex.Message));
        }
    }

    // DELETE: api/Invoices/5
    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<object>>> Delete(int id)
    {
        var result = await _invoiceService.DeleteAsync(id);

        if (!result)
            return NotFound(ApiResponse<object>.NotFoundResponse("Factura no encontrada"));

        return Ok(ApiResponse<object>.SuccessResponse("Factura eliminada exitosamente"));
    }
}

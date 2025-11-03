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
public class CustomersController : ControllerBase
{
    private readonly ICustomerService _customerService;

    public CustomersController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<CustomerDto>>>> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? search = null)
    {
        var (customers, total) = await _customerService.GetAllAsync(page, pageSize, search);

        Response.Headers["X-Total-Count"] = total.ToString();
        Response.Headers["X-Page"] = page.ToString();
        Response.Headers["X-Page-Size"] = pageSize.ToString();

        return Ok(ApiResponse<IEnumerable<CustomerDto>>.SuccessResponse(
            customers,
            $"Se encontraron {total} clientes"
        ));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<CustomerDto>>> GetById(int id)
    {
        var customer = await _customerService.GetByIdAsync(id);

        if (customer == null)
            return NotFound(ApiResponse<CustomerDto>.NotFoundResponse("Cliente no encontrado"));

        return Ok(ApiResponse<CustomerDto>.SuccessResponse(customer, "Cliente obtenido exitosamente"));
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<CustomerDto>>> Create([FromBody] CreateCustomerDto dto)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();
            return BadRequest(ApiResponse<CustomerDto>.ValidationErrorResponse(errors));
        }

        try
        {
            var customer = await _customerService.CreateAsync(dto);
            return CreatedAtAction(
                nameof(GetById),
                new { id = customer.IdCustomer },
                ApiResponse<CustomerDto>.SuccessResponse(customer, "Cliente creado exitosamente")
            );
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ApiResponse<CustomerDto>.ErrorResponse(ex.Message));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<CustomerDto>.ErrorResponse($"Error interno: {ex.Message}"));
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<CustomerDto>>> Update(int id, [FromBody] UpdateCustomerDto dto)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();
            return BadRequest(ApiResponse<CustomerDto>.ValidationErrorResponse(errors));
        }

        try
        {
            var customer = await _customerService.UpdateAsync(id, dto);

            if (customer == null)
                return NotFound(ApiResponse<CustomerDto>.NotFoundResponse("Cliente no encontrado"));

            return Ok(ApiResponse<CustomerDto>.SuccessResponse(customer, "Cliente actualizado exitosamente"));
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ApiResponse<CustomerDto>.ErrorResponse(ex.Message));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<CustomerDto>.ErrorResponse($"Error interno: {ex.Message}"));
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<object>>> Delete(int id)
    {
        var result = await _customerService.DeleteAsync(id);

        if (!result)
            return NotFound(ApiResponse<object>.NotFoundResponse("Cliente no encontrado"));

        return Ok(ApiResponse<object>.SuccessResponse("Cliente eliminado exitosamente"));
    }
}
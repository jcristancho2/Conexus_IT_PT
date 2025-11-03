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
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    // GET: api/Products
    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<ProductDto>>>> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? search = null)
    {
        var (products, total) = await _productService.GetAllAsync(page, pageSize, search);

        Response.Headers["X-Total-Count"] = total.ToString();
        Response.Headers["X-Page"] = page.ToString();
        Response.Headers["X-Page-Size"] = pageSize.ToString();

        return Ok(ApiResponse<IEnumerable<ProductDto>>.SuccessResponse(
            products,
            $"Se encontraron {total} productos"
        ));
    }

    // GET: api/Products/5
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<ProductDto>>> GetById(int id)
    {
        var product = await _productService.GetByIdAsync(id);

        if (product == null)
            return NotFound(ApiResponse<ProductDto>.NotFoundResponse("Producto no encontrado"));

        return Ok(ApiResponse<ProductDto>.SuccessResponse(product, "Producto obtenido exitosamente"));
    }

    // POST: api/Products
    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductDto dto)
    {
        try
        {
            var product = await _productService.CreateAsync(dto);

            return Ok(ApiResponse<ProductDto>.SuccessResponse(
                product, 
                "Producto creado exitosamente"
            ));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<object>.ErrorResponse(ex.Message));
        }
    }

    // PUT: api/Products/5
    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<ProductDto>>> Update(int id, [FromBody] UpdateProductDto dto)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();
            return BadRequest(ApiResponse<ProductDto>.ValidationErrorResponse(errors));
        }

        try
        {
            var product = await _productService.UpdateAsync(id, dto);

            if (product == null)
                return NotFound(ApiResponse<ProductDto>.NotFoundResponse("Producto no encontrado"));

            return Ok(ApiResponse<ProductDto>.SuccessResponse(product, "Producto actualizado exitosamente"));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ApiResponse<ProductDto>.ErrorResponse(ex.Message));
        }
    }

    // DELETE: api/Products/5
    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<object>>> Delete(int id)
    {
        var result = await _productService.DeleteAsync(id);

        if (!result)
            return NotFound(ApiResponse<object>.NotFoundResponse("Producto no encontrado"));

        return Ok(ApiResponse<object>.SuccessResponse("Producto eliminado exitosamente"));
    }
}
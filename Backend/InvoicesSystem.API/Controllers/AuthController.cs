using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using InvoicesSystem.API.Services.Interfaces;
using InvoicesSystem.API.Models.DTOs;
using InvoicesSystem.API.Models.Responses;

namespace InvoicesSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<ActionResult<ApiResponse<LoginResponse>>> Login([FromBody] LoginRequest request)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();
            return BadRequest(ApiResponse<LoginResponse>.ValidationErrorResponse(errors));
        }

        var result = await _authService.LoginAsync(request.Email, request.Password);

        if (result == null)
            return Unauthorized(ApiResponse<LoginResponse>.UnauthorizedResponse("Credenciales inválidas"));

        return Ok(ApiResponse<LoginResponse>.SuccessResponse(result, "Inicio de sesión exitoso"));
    }

    [HttpPost("register")]
    public async Task<ActionResult<ApiResponse<LoginResponse>>> Register([FromBody] RegisterRequest request)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();
            return BadRequest(ApiResponse<LoginResponse>.ValidationErrorResponse(errors));
        }

        var result = await _authService.RegisterAsync(request);

        if (result == null)
            return BadRequest(ApiResponse<LoginResponse>.ErrorResponse("El usuario ya existe o hubo un error en el registro"));

        return Ok(ApiResponse<LoginResponse>.SuccessResponse(result, "Registro exitoso"));
    }
}
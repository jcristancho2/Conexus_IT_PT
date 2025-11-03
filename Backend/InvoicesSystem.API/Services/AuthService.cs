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
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == email && u.IsActive);

        if (user == null || user.PasswordHash != password)
            return null;

        var token = GenerateJwtToken(user);
        var expirationMinutes = _configuration.GetValue<int>("Jwt:ExpirationInMinutes", 1440);

        return new LoginResponse
        {
            Token = token,
            Expiration = DateTime.UtcNow.AddMinutes(expirationMinutes),
            User = new UserDto
            {
                IdUser = user.IdUser,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Role = user.Role,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt
            }
        };
    }

    public async Task<LoginResponse?> RegisterAsync(RegisterRequest request)
    {
        var existingUser = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == request.Email);

        if (existingUser != null)
            return null;

        var user = new User
        {
            Email = request.Email,
            PasswordHash = request.Password,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Role = request.Role,
            CreatedAt = DateTime.UtcNow
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var token = GenerateJwtToken(user);
        var expirationMinutes = _configuration.GetValue<int>("Jwt:ExpirationInMinutes", 1440);

        return new LoginResponse
        {
            Token = token,
            Expiration = DateTime.UtcNow.AddMinutes(expirationMinutes),
            User = new UserDto
            {
                IdUser = user.IdUser,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Role = user.Role,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt
            }
        };
    }

    public async Task<User?> GetUserByEmailAsync(string email) // Cambiado de Customer a User
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    public string GenerateJwtToken(User user) // Cambiado de Customer a User
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
            new Claim(ClaimTypes.NameIdentifier, user.IdUser.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
            new Claim(ClaimTypes.Role, user.Role)
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
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InvoicesSystem.API.Models.DTOs;
using InvoicesSystem.API.Models.Entities;

namespace InvoicesSystem.API.Services.Interfaces;

public interface IAuthService
{
    Task<LoginResponse?> LoginAsync(string email, string password);
    Task<LoginResponse?> RegisterAsync(RegisterRequest request);
    Task<User?> GetUserByEmailAsync(string email);
    string GenerateJwtToken(User user);
}
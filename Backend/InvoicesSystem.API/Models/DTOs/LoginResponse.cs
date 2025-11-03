using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvoicesSystem.API.Models.DTOs;

public class LoginResponse
{
    public string Token { get; set; } = null!;
    public DateTime Expiration { get; set; }
    public UserDto User { get; set; } = null!; 
}
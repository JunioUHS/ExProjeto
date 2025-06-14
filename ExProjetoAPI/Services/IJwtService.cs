using ExProjetoAPI.Models;
using System.Security.Claims;

namespace ExProjetoAPI.Services
{
    public interface IJwtService
    {
        string GenerateToken(ApplicationUser user);
        ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
    }
}
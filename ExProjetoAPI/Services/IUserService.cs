using ExProjetoAPI.DTOs;
using ExProjetoAPI.Models;
using Microsoft.AspNetCore.Identity;

namespace ExProjetoAPI.Services
{
    public interface IUserService
    {
        Task<IdentityResult> RegisterAsync(RegisterUserDto dto);
        Task<(string? token, string? refreshToken)> LoginAsync(LoginUserDto dto);
        Task<(string? token, string? refreshToken)> RefreshTokenAsync(string refreshToken);
        Task LogoutAsync(string refreshToken);
    }
}
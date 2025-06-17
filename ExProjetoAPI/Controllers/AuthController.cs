using ExProjetoAPI.DTOs;
using ExProjetoAPI.Extensions;
using ExProjetoAPI.Models;
using ExProjetoAPI.Responses;
using ExProjetoAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExProjetoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginUserDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<IEnumerable<ApiError>>.Fail(ModelState.ToApiErrors()));

            var (token, refreshToken) = await _userService.LoginAsync(dto);

            if (token == null)
                return Unauthorized(ApiResponse<object>.Fail("Usuário ou senha inválidos."));

            // Setar o refresh token como cookie HttpOnly e Secure
            Response.Cookies.Append("refreshToken", refreshToken!, new CookieOptions
            {
                HttpOnly = true,
                Secure = true, // true em produção (HTTPS)
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddDays(UserRefreshToken.DefaultExpirationDays)
            });

            return Ok(ApiResponse<string>.Ok(token, "Login feito com sucesso."));
        }

        [HttpPost("refresh-token")]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshToken()
        {
            // Pega o refresh token do cookie
            var refreshToken = Request.Cookies["refreshToken"];
            if (string.IsNullOrEmpty(refreshToken))
                return Unauthorized(ApiResponse<object>.Fail("Refresh token não encontrado."));

            var (token, newRefreshToken) = await _userService.RefreshTokenAsync(refreshToken);

            if (token == null)
                return Unauthorized(ApiResponse<object>.Fail("Refresh token inválido ou expirado."));

            // Atualiza o cookie com o novo refresh token
            Response.Cookies.Append("refreshToken", newRefreshToken!, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddDays(UserRefreshToken.DefaultExpirationDays)
            });

            return Ok(ApiResponse<string>.Ok(token, "Token renovado com sucesso."));
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            if (string.IsNullOrEmpty(refreshToken))
                return Ok(ApiResponse<string>.Ok("Logout realizado."));

            await _userService.LogoutAsync(refreshToken);

            // Remove o cookie do refresh token
            Response.Cookies.Delete("refreshToken");

            return Ok(ApiResponse<string>.Ok("Logout realizado com sucesso."));
        }
    }
}

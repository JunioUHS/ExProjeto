using ExProjetoAPI.DTOs;
using ExProjetoAPI.Extensions;
using ExProjetoAPI.Responses;
using ExProjetoAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExProjetoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<object>.Fail(ModelState.ToApiErrors()));

            var result = await _userService.RegisterAsync(dto);

            if (!result.Succeeded)
                return BadRequest(ApiResponse<object>.Fail(result.ToApiErrors()));

            return Ok(ApiResponse<object>.Ok("Login feito com sucesso."));
        }
    }
}

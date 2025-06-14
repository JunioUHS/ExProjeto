using ExProjetoAPI.DTOs;
using ExProjetoAPI.Models;
using ExProjetoAPI.Repositories;
using Microsoft.AspNetCore.Identity;
using AutoMapper;

namespace ExProjetoAPI.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJwtService _jwtService;
        private readonly IMapper _mapper;
        private readonly IUserRefreshTokenRepository _refreshTokenRepository;

        public UserService(
            UserManager<ApplicationUser> userManager,
            IJwtService jwtService,
            IMapper mapper,
            IUserRefreshTokenRepository refreshTokenRepository)
        {
            _userManager = userManager;
            _jwtService = jwtService;
            _mapper = mapper;
            _refreshTokenRepository = refreshTokenRepository;
        }

        public async Task<IdentityResult> RegisterAsync(RegisterUserDto dto)
        {
            var user = _mapper.Map<ApplicationUser>(dto);
            return await _userManager.CreateAsync(user, dto.Password);
        }

        public async Task<(string? token, string? refreshToken)> LoginAsync(LoginUserDto dto)
        {
            var user = await _userManager.FindByNameAsync(dto.UserName);
            if (user == null || !await _userManager.CheckPasswordAsync(user, dto.Password))
                return (null, null);

            var token = _jwtService.GenerateToken(user);

            var userRefreshToken = UserRefreshToken.Create(user);

            await _refreshTokenRepository.AddAsync(userRefreshToken);
            await _refreshTokenRepository.SaveChangesAsync();

            return (token, userRefreshToken.Token);
        }

        public async Task<(string? token, string? refreshToken)> RefreshTokenAsync(string refreshToken)
        {
            var userRefreshToken = await _refreshTokenRepository.GetByTokenAsync(refreshToken);

            if (userRefreshToken == null || userRefreshToken.Expiration <= DateTime.UtcNow)
                return (null, null);

            await _refreshTokenRepository.RevokeAsync(userRefreshToken);

            var user = userRefreshToken.User;
            var newToken = _jwtService.GenerateToken(user);

            var newUserRefreshToken = UserRefreshToken.Create(user);

            await _refreshTokenRepository.AddAsync(newUserRefreshToken);
            await _refreshTokenRepository.SaveChangesAsync();

            return (newToken, newUserRefreshToken.Token);
        }

        public async Task LogoutAsync(string refreshToken)
        {
            var userRefreshToken = await _refreshTokenRepository.GetByTokenAsync(refreshToken);
            if (userRefreshToken != null)
            {
                await _refreshTokenRepository.RevokeAsync(userRefreshToken);
                await _refreshTokenRepository.SaveChangesAsync();
            }
        }
    }
}
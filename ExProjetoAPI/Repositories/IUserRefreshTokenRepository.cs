using ExProjetoAPI.Models;

namespace ExProjetoAPI.Repositories
{
    public interface IUserRefreshTokenRepository
    {
        Task AddAsync(UserRefreshToken refreshToken);
        Task<UserRefreshToken?> GetByTokenAsync(string token);
        Task RevokeAsync(UserRefreshToken refreshToken);
        Task SaveChangesAsync();
    }
}
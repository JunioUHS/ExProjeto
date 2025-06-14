using ExProjetoAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ExProjetoAPI.Repositories
{
    public class UserRefreshTokenRepository : IUserRefreshTokenRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRefreshTokenRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(UserRefreshToken refreshToken)
        {
            await _context.UserRefreshTokens.AddAsync(refreshToken);
        }

        public async Task<UserRefreshToken?> GetByTokenAsync(string token)
        {
            return await _context.UserRefreshTokens
                .Include(rt => rt.User)
                .FirstOrDefaultAsync(rt => rt.Token == token && !rt.IsRevoked);
        }

        public async Task RevokeAsync(UserRefreshToken refreshToken)
        {
            refreshToken.IsRevoked = true;
            await Task.Run(() => _context.UserRefreshTokens.Update(refreshToken));
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
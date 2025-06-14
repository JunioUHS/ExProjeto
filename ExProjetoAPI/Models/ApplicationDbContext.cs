using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace ExProjetoAPI.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<UserRefreshToken> UserRefreshTokens { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<UserRefreshToken>(entity =>
            {
                entity.HasKey(t => t.Id);

                entity.HasOne(t => t.User)
                      .WithMany(u => u.RefreshTokens)
                      .HasForeignKey(t => t.UserId)
                      .IsRequired();
            });
        }
    }
}
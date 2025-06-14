using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExProjetoAPI.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Column(TypeName = "nvarchar(150)")]
        public required string FullName { get; set; }
        public ICollection<UserRefreshToken> RefreshTokens { get; set; } = new List<UserRefreshToken>();
    }
}
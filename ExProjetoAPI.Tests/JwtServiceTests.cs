using Xunit;
using Moq;
using Microsoft.Extensions.Configuration;
using ExProjetoAPI.Services;
using ExProjetoAPI.Models;

namespace ExProjetoAPI.Tests
{
    public class JwtServiceTests
    {
        [Fact]
        public void GenerateToken_ReturnsToken()
        {
            // Arrange
            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                { "Jwt:Key", "SuaChaveSuperSecretaComMaisDe32Caracteres" },
                { "Jwt:Issuer", "issuer" },
                { "Jwt:Audience", "audience" }
                })
                .Build();

            var service = new JwtService(config);
            var user = new ApplicationUser { Id = "1", UserName = "user", FullName = "User Teste" };

            // Act
            var token = service.GenerateToken(user);

            // Assert
            Assert.False(string.IsNullOrEmpty(token));
        }

        [Fact]
        public void GenerateToken_ComChaveCurta_LancaExcecao()
        {
            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    { "Jwt:Key", "chavecurta" },
                    { "Jwt:Issuer", "issuer" },
                    { "Jwt:Audience", "audience" }
                })
                .Build();

            var service = new JwtService(config);
            var user = new ApplicationUser { Id = "1", UserName = "user", FullName = "User Teste" };

            Assert.Throws<ArgumentOutOfRangeException>(() => service.GenerateToken(user));
        }
    }
}
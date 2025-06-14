using System.Net.Http.Json;
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using ExProjetoAPI.DTOs;

namespace ExProjetoAPI.Tests
{
    public class AuthControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public AuthControllerTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Login_ComDadosInvalidos_RetornaBadRequest()
        {
            // Arrange
            var dto = new LoginUserDto { UserName = "", Password = "" };

            // Act
            var response = await _client.PostAsJsonAsync("/api/Auth/login", dto);

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
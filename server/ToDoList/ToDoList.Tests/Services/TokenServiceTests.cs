using Microsoft.Extensions.Configuration;
using ToDoList.API.Models;
using ToDoList.API.Services;

namespace ToDoList.Tests.Services
{
    public class TokenServiceTests
    {
        private readonly TokenService _tokenService;

        public TokenServiceTests()
        {
            var inMemorySettings = new Dictionary<string, string> {
                {"Jwt:Key", "DevHmacSha256SecretKey@1234!Secure"},
                {"Jwt:Issuer", "ToDoListAPI"},
                {"Jwt:Audience", "ToDoListAPIUsers"}
            };

            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            _tokenService = new TokenService(configuration);
        }

        [Fact]
        public void GenerateToken_ValidUser_ReturnsToken()
        {
            // Arrange
            var user = new User
            {
                Username = "admin",
                Password = "password1"
            };

            // Act
            var token = _tokenService.GenerateToken(user);

            // Assert
            Assert.False(string.IsNullOrEmpty(token));
        }
    }
}

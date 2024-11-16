using Microsoft.AspNetCore.Mvc;
using Moq;
using ToDoList.API.Controllers;
using ToDoList.API.Models;
using ToDoList.API.Repositories;
using ToDoList.API.Services;

namespace ToDoList.Tests.Controllers
{
    public class UserControllerTests
    {
        private readonly Mock<IRepository<User>> _userRepositoryMock;
        private readonly Mock<ITokenService> _tokenServiceMock;
        private readonly UserController _userController;

        public UserControllerTests()
        {
            _userRepositoryMock = new Mock<IRepository<User>>();
            _tokenServiceMock = new Mock<ITokenService>();
            _userController = new UserController(_userRepositoryMock.Object, _tokenServiceMock.Object);
        }

        [Fact]
        public async Task Login_ValidCredentials_ReturnsToken()
        {
            // Arrange
            var loginUser = new User { Username = "admin", Password = "password1" };
            var user = new User { UserId = Guid.NewGuid(), Username = "admin", Password = "password1" };

            _userRepositoryMock.Setup(repo => repo.FindAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<User, bool>>>()))
                .ReturnsAsync(new List<User> { user });

            _tokenServiceMock.Setup(service => service.GenerateToken(It.IsAny<User>())).Returns("testtoken");

            // Act
            var result = await _userController.Login(loginUser);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("testtoken", ((dynamic)okResult.Value).Token);
        }

        [Fact]
        public async Task Login_InvalidCredentials_ReturnsUnauthorized()
        {
            // Arrange
            var loginUser = new User { Username = "admin", Password = "wrongpassword" };

            _userRepositoryMock.Setup(repo => repo.FindAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<User, bool>>>()))
                .ReturnsAsync(Enumerable.Empty<User>());

            // Act
            var result = await _userController.Login(loginUser);

            // Assert
            Assert.IsType<UnauthorizedObjectResult>(result);
        }
    }
}

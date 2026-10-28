using FurnitureSalon.Api.Interfaces;
using FurnitureSalon.Api.Models;
using FurnitureSalon.Api.Services;
using FurnitureSalon.Core.DTO.Login;
using Moq;

namespace FurnitureSalon.xUnit
{
    public class AuthServiceTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly AuthService _authService;

        public AuthServiceTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _authService = new AuthService(_userRepositoryMock.Object);
        }

        [Fact]
        public async Task LoginAsync_ValidLoginAndPassword_ReturnsIsSuccessTrue()
        {
            // Arrange
            var user = new User
            {
                Login = "test",
                Password = "1234",
                Role = "TestRole",
                IdEmployeeNavigation = new Employee
                {
                    Surname = "Test",
                    Name = "Test",
                    Patronymic = "Test",
                    EmployeeRole = "Test"
                }
            };

            _userRepositoryMock
                .Setup(r => r.GetByLoginAsync("test"))
                .ReturnsAsync(user);

            var request = new LoginRequest
            {
                Login = "test",
                Password = "1234"
            };

            // Act
            var result = await _authService.LoginAsync(request);

            // Assert
            Assert.True(result?.IsSuccess);
        }

        [Fact]
        public async Task LoginAsync_InvalidLoginAndPassword_ReturnsIsSuccessFalse()
        {
            // Arrange
            var user = new User
            {
                Login = "test",
                Password = "1234"
            };

            _userRepositoryMock
                .Setup(r => r.GetByLoginAsync("test"))
                .ReturnsAsync(user);

            var request = new LoginRequest
            {
                Login = "test",
                Password = "wrong"
            };

            // Act
            var result = await _authService.LoginAsync(request);

            // Assert
            Assert.False(result?.IsSuccess);
        }

        [Fact]
        public async Task LoginAsync_UserNotFound_ReturnsIsSuccessFalse()
        {
            // Arrange
            _userRepositoryMock
                .Setup(r => r.GetByLoginAsync("unknown"))
                .ReturnsAsync((User?)null);

            var request = new LoginRequest
            {
                Login = "unknown",
                Password = "1234"
            };

            // Act
            var result = await _authService.LoginAsync(request);

            // Assert
            Assert.False(result?.IsSuccess);
        }
    }
}
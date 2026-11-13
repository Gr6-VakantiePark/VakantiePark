using Moq;
using VakantiePark.Application.Services;
using VakantiePark.Domain.Entities;
using VakantiePark.Domain.Interfaces;
using VakantiePark.Shared.DTOs.Auth;

namespace VakantiePark.UnitTests.Services
{
    public class AuthServiceTests
    {
        private readonly Mock<IEmployeeRepository> _repository = new();
        private readonly Mock<IRepository<Employee>> _employees = new();
        private readonly Mock<IPasswordHasher> _passwordHasher = new();
        private readonly Mock<ITokenService> _tokenService = new();
        private readonly AuthService _sut;

        public AuthServiceTests()
        {
            _sut = new AuthService(_repository.Object, _employees.Object, _passwordHasher.Object, _tokenService.Object);
        }

        private static Employee CreateEmployee(bool isActive = true) => new()
        {
            Id = Guid.NewGuid(),
            Email = "receptie@example.com",
            PasswordHash = "hash",
            FirstName = "Sanne",
            LastName = "de Vries",
            IsActive = isActive,
        };

        private static LoginRequestDto CreateRequest(string email = "Receptie@Example.com ", string password = "Zomer2027!Vakantie") => new()
        {
            Email = email,
            Password = password,
        };

        [Fact]
        public async Task LoginAsync_ValidCredentials_ReturnsTokenAndSavesLastLogin()
        {
            var employee = CreateEmployee();
            var expiresAt = DateTime.UtcNow.AddHours(1);
            _repository.Setup(r => r.GetByEmailAsync("receptie@example.com")).ReturnsAsync(employee);
            _passwordHasher.Setup(h => h.Verify("Zomer2027!Vakantie", "hash")).Returns(true);
            _tokenService.Setup(t => t.CreateToken(employee)).Returns(("token", expiresAt));

            var result = await _sut.LoginAsync(CreateRequest());

            Assert.Equal("token", result.Token);
            Assert.Equal(expiresAt, result.ExpiresAt);
            Assert.NotNull(employee.LastLoginAt);
            _employees.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Theory]
        [InlineData("", "Zomer2027!Vakantie")]
        [InlineData("receptie@example.com", "")]
        public async Task LoginAsync_MissingCredentials_ThrowsArgumentException(string email, string password)
        {
            await Assert.ThrowsAsync<ArgumentException>(() => _sut.LoginAsync(CreateRequest(email, password)));

            _repository.Verify(r => r.GetByEmailAsync(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task LoginAsync_NullRequest_ThrowsArgumentException()
        {
            await Assert.ThrowsAsync<ArgumentException>(() => _sut.LoginAsync(null!));
        }

        [Fact]
        public async Task LoginAsync_UnknownEmail_ThrowsUnauthorizedAccessException()
        {
            _repository.Setup(r => r.GetByEmailAsync(It.IsAny<string>())).ReturnsAsync((Employee?)null);
            _passwordHasher.Setup(h => h.Hash(It.IsAny<string>())).Returns("dummy");

            await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _sut.LoginAsync(CreateRequest()));

            _employees.Verify(r => r.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task LoginAsync_WrongPassword_ThrowsUnauthorizedAccessException()
        {
            _repository.Setup(r => r.GetByEmailAsync(It.IsAny<string>())).ReturnsAsync(CreateEmployee());
            _passwordHasher.Setup(h => h.Verify(It.IsAny<string>(), It.IsAny<string>())).Returns(false);

            await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _sut.LoginAsync(CreateRequest()));

            _tokenService.Verify(t => t.CreateToken(It.IsAny<Employee>()), Times.Never);
        }

        [Fact]
        public async Task LoginAsync_InactiveEmployee_ThrowsUnauthorizedAccessException()
        {
            _repository.Setup(r => r.GetByEmailAsync(It.IsAny<string>())).ReturnsAsync(CreateEmployee(isActive: false));
            _passwordHasher.Setup(h => h.Verify(It.IsAny<string>(), It.IsAny<string>())).Returns(true);

            var exception = await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _sut.LoginAsync(CreateRequest()));

            Assert.Equal(AuthService.AccountDisabled, exception.Message);
            _tokenService.Verify(t => t.CreateToken(It.IsAny<Employee>()), Times.Never);
        }
    }
}

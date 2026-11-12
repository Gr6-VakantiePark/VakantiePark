using Moq;
using VakantiePark.Application.Interfaces;
using VakantiePark.Application.Services;
using VakantiePark.Domain.Entities;
using VakantiePark.Domain.Interfaces;
using VakantiePark.Shared.DTOs.Auth;

namespace VakantiePark.UnitTests.Services
{
    public class EmployeeServiceTests
    {
        private readonly Mock<IEmployeeRepository> _repository = new();
        private readonly Mock<IPasswordHasher> _passwordHasher = new();
        private readonly EmployeeService _sut;

        public EmployeeServiceTests()
        {
            _sut = new EmployeeService(_repository.Object, _passwordHasher.Object);
            _passwordHasher.Setup(h => h.Hash(It.IsAny<string>())).Returns("hash");
        }

        private static CreateEmployeeDto CreateRequest() => new()
        {
            Email = " Nieuw@Example.com ",
            Password = "Zomer2027!Vakantie",
            FirstName = " Sanne ",
            LastName = "de Vries",
            Roles = ["reception"],
        };

        private static Role CreateRole(string code = "reception") => new() { Id = Guid.NewGuid(), Code = code, Name = "Receptie" };

        [Fact]
        public async Task CreateAsync_ValidRequest_AddsEmployeeOnce()
        {
            Employee? added = null;
            _repository.Setup(r => r.EmailExistsAsync("nieuw@example.com")).ReturnsAsync(false);
            _repository.Setup(r => r.GetRolesAsync(It.IsAny<IEnumerable<string>>())).ReturnsAsync([CreateRole()]);
            _repository.Setup(r => r.AddAsync(It.IsAny<Employee>())).Callback<Employee>(e => added = e).Returns(Task.CompletedTask);

            var result = await _sut.CreateAsync(CreateRequest());

            Assert.NotNull(added);
            Assert.Equal("nieuw@example.com", added.Email);
            Assert.Equal("Sanne", added.FirstName);
            Assert.Equal("hash", added.PasswordHash);
            Assert.Equal(new[] { "reception" }, result.Roles);
            _repository.Verify(r => r.AddAsync(It.IsAny<Employee>()), Times.Once);
            _repository.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_RoleWithSpaces_IsNotReportedAsUnknown()
        {
            var request = CreateRequest();
            request.Roles = [" reception "];
            _repository.Setup(r => r.GetRolesAsync(It.IsAny<IEnumerable<string>>())).ReturnsAsync([CreateRole()]);

            var result = await _sut.CreateAsync(request);

            Assert.Equal(new[] { "reception" }, result.Roles);
        }

        [Fact]
        public async Task CreateAsync_EmailExists_ThrowsArgumentException()
        {
            _repository.Setup(r => r.EmailExistsAsync(It.IsAny<string>())).ReturnsAsync(true);

            await Assert.ThrowsAsync<ArgumentException>(() => _sut.CreateAsync(CreateRequest()));

            _repository.Verify(r => r.AddAsync(It.IsAny<Employee>()), Times.Never);
        }

        [Fact]
        public async Task CreateAsync_UnknownRole_ThrowsArgumentException()
        {
            _repository.Setup(r => r.GetRolesAsync(It.IsAny<IEnumerable<string>>())).ReturnsAsync([]);

            await Assert.ThrowsAsync<ArgumentException>(() => _sut.CreateAsync(CreateRequest()));

            _repository.Verify(r => r.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task CreateAsync_InvalidRequest_ThrowsBeforeRepositoryIsUsed()
        {
            var request = CreateRequest();
            request.Password = "kort";

            await Assert.ThrowsAsync<ArgumentException>(() => _sut.CreateAsync(request));

            _repository.Verify(r => r.EmailExistsAsync(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task GetByIdAsync_NotFound_ThrowsKeyNotFoundException()
        {
            _repository.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Employee?)null);

            await Assert.ThrowsAsync<KeyNotFoundException>(() => _sut.GetByIdAsync(Guid.NewGuid()));
        }

        [Fact]
        public async Task GetByIdAsync_EmptyId_ThrowsArgumentException()
        {
            await Assert.ThrowsAsync<ArgumentException>(() => _sut.GetByIdAsync(Guid.Empty));

            _repository.Verify(r => r.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
        }
    }
}

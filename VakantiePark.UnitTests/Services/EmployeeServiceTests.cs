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
        private readonly Mock<IRepository<Employee>> _employees = new();
        private readonly Mock<IPasswordHasher> _passwordHasher = new();
        private readonly EmployeeService _sut;

        public EmployeeServiceTests()
        {
            _sut = new EmployeeService(_repository.Object, _employees.Object, _passwordHasher.Object);
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
            _employees.Setup(r => r.Add(It.IsAny<Employee>())).Callback<Employee>(e => added = e);

            var result = await _sut.CreateAsync(CreateRequest());

            Assert.NotNull(added);
            Assert.Equal("nieuw@example.com", added.Email);
            Assert.Equal("Sanne", added.FirstName);
            Assert.Equal("hash", added.PasswordHash);
            Assert.Equal(new[] { "reception" }, result.Roles);
            _employees.Verify(r => r.Add(It.IsAny<Employee>()), Times.Once);
            _employees.Verify(r => r.SaveChangesAsync(), Times.Once);
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

            _employees.Verify(r => r.Add(It.IsAny<Employee>()), Times.Never);
        }

        [Fact]
        public async Task CreateAsync_UnknownRole_ThrowsArgumentException()
        {
            _repository.Setup(r => r.GetRolesAsync(It.IsAny<IEnumerable<string>>())).ReturnsAsync([]);

            await Assert.ThrowsAsync<ArgumentException>(() => _sut.CreateAsync(CreateRequest()));

            _employees.Verify(r => r.SaveChangesAsync(), Times.Never);
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

        private static Employee CreateEmployee(bool isActive = true) => new()
        {
            Id = Guid.NewGuid(),
            Email = "oud@example.com",
            FirstName = "Oud",
            LastName = "Naam",
            IsActive = isActive,
        };

        private static UpdateEmployeeDto UpdateRequest() => new()
        {
            Email = " Nieuw@Example.com ",
            FirstName = " Sanne ",
            LastName = "de Vries",
        };

        [Fact]
        public async Task UpdateAsync_ValidRequest_UpdatesEmployeeAndSavesOnce()
        {
            var employee = CreateEmployee();
            _repository.Setup(r => r.GetForUpdateAsync(employee.Id)).ReturnsAsync(employee);
            _repository.Setup(r => r.EmailExistsAsync("nieuw@example.com", employee.Id)).ReturnsAsync(false);

            var result = await _sut.UpdateAsync(employee.Id, UpdateRequest());

            Assert.Equal("nieuw@example.com", employee.Email);
            Assert.Equal("Sanne", employee.FirstName);
            Assert.Equal("de Vries", result.LastName);
            _employees.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_EmailUsedByOtherEmployee_ThrowsArgumentException()
        {
            var employee = CreateEmployee();
            _repository.Setup(r => r.GetForUpdateAsync(employee.Id)).ReturnsAsync(employee);
            _repository.Setup(r => r.EmailExistsAsync("nieuw@example.com", employee.Id)).ReturnsAsync(true);

            await Assert.ThrowsAsync<ArgumentException>(() => _sut.UpdateAsync(employee.Id, UpdateRequest()));

            Assert.Equal("oud@example.com", employee.Email);
            _employees.Verify(r => r.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task UpdateAsync_NotFound_ThrowsKeyNotFoundException()
        {
            _repository.Setup(r => r.GetForUpdateAsync(It.IsAny<Guid>())).ReturnsAsync((Employee?)null);

            await Assert.ThrowsAsync<KeyNotFoundException>(() => _sut.UpdateAsync(Guid.NewGuid(), UpdateRequest()));
        }

        [Fact]
        public async Task UpdateAsync_InvalidEmail_ThrowsBeforeRepositoryIsUsed()
        {
            var request = UpdateRequest();
            request.Email = "geen-e-mailadres";

            await Assert.ThrowsAsync<ArgumentException>(() => _sut.UpdateAsync(Guid.NewGuid(), request));

            _repository.Verify(r => r.GetForUpdateAsync(It.IsAny<Guid>()), Times.Never);
        }

        [Fact]
        public async Task SetActiveAsync_Deactivate_SetsInactiveAndSavesOnce()
        {
            var employee = CreateEmployee();
            _repository.Setup(r => r.GetForUpdateAsync(employee.Id)).ReturnsAsync(employee);

            var result = await _sut.SetActiveAsync(employee.Id, new SetEmployeeActiveDto { IsActive = false }, Guid.NewGuid());

            Assert.False(employee.IsActive);
            Assert.False(result.IsActive);
            _employees.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task SetActiveAsync_Activate_SetsActive()
        {
            var employee = CreateEmployee(isActive: false);
            _repository.Setup(r => r.GetForUpdateAsync(employee.Id)).ReturnsAsync(employee);

            await _sut.SetActiveAsync(employee.Id, new SetEmployeeActiveDto { IsActive = true }, Guid.NewGuid());

            Assert.True(employee.IsActive);
        }

        [Fact]
        public async Task SetActiveAsync_DeactivateOwnAccount_ThrowsArgumentException()
        {
            var employee = CreateEmployee();

            await Assert.ThrowsAsync<ArgumentException>(() =>
                _sut.SetActiveAsync(employee.Id, new SetEmployeeActiveDto { IsActive = false }, employee.Id));

            _employees.Verify(r => r.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task SetActiveAsync_SameStatus_DoesNotSave()
        {
            var employee = CreateEmployee();
            _repository.Setup(r => r.GetForUpdateAsync(employee.Id)).ReturnsAsync(employee);

            await _sut.SetActiveAsync(employee.Id, new SetEmployeeActiveDto { IsActive = true }, Guid.NewGuid());

            _employees.Verify(r => r.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task SetActiveAsync_NotFound_ThrowsKeyNotFoundException()
        {
            _repository.Setup(r => r.GetForUpdateAsync(It.IsAny<Guid>())).ReturnsAsync((Employee?)null);

            await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                _sut.SetActiveAsync(Guid.NewGuid(), new SetEmployeeActiveDto { IsActive = false }, Guid.NewGuid()));
        }
    }
}

using Moq;
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
            Permissions = ["guests.view"],
        };

        private static Permission CreatePermission(string code = "guests.view") => new() { Id = Guid.NewGuid(), Code = code, Name = "Gastgegevens bekijken" };

        [Fact]
        public async Task CreateAsync_ValidRequest_AddsEmployeeOnce()
        {
            Employee? added = null;
            _repository.Setup(r => r.EmailExistsAsync("nieuw@example.com")).ReturnsAsync(false);
            _repository.Setup(r => r.GetPermissionsAsync(It.IsAny<IEnumerable<string>>())).ReturnsAsync([CreatePermission()]);
            _employees.Setup(r => r.Add(It.IsAny<Employee>())).Callback<Employee>(e => added = e);

            var result = await _sut.CreateAsync(CreateRequest());

            Assert.NotNull(added);
            Assert.Equal("nieuw@example.com", added.Email);
            Assert.Equal("Sanne", added.FirstName);
            Assert.Equal("hash", added.PasswordHash);
            Assert.Equal(new[] { "guests.view" }, result.Permissions);
            Assert.False(added.IsOwner);
            _employees.Verify(r => r.Add(It.IsAny<Employee>()), Times.Once);
            _employees.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_PermissionWithSpaces_IsNotReportedAsUnknown()
        {
            var request = CreateRequest();
            request.Permissions = [" Guests.View "];
            _repository.Setup(r => r.GetPermissionsAsync(It.IsAny<IEnumerable<string>>())).ReturnsAsync([CreatePermission()]);

            var result = await _sut.CreateAsync(request);

            Assert.Equal(new[] { "guests.view" }, result.Permissions);
        }

        [Fact]
        public async Task CreateAsync_EmailExists_ThrowsArgumentException()
        {
            _repository.Setup(r => r.EmailExistsAsync(It.IsAny<string>())).ReturnsAsync(true);

            await Assert.ThrowsAsync<ArgumentException>(() => _sut.CreateAsync(CreateRequest()));

            _employees.Verify(r => r.Add(It.IsAny<Employee>()), Times.Never);
        }

        [Fact]
        public async Task CreateAsync_UnknownPermission_ThrowsArgumentException()
        {
            _repository.Setup(r => r.GetPermissionsAsync(It.IsAny<IEnumerable<string>>())).ReturnsAsync([]);

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

        [Fact]
        public async Task UpdatePermissionsAsync_ValidRequest_ReplacesPermissionsAndSavesOnce()
        {
            var employee = CreateEmployee();
            employee.Permissions.Add(CreatePermission("pool.capacity.manage"));
            _repository.Setup(r => r.GetForUpdateAsync(employee.Id)).ReturnsAsync(employee);
            _repository.Setup(r => r.GetPermissionsAsync(It.IsAny<IEnumerable<string>>()))
                .ReturnsAsync([CreatePermission("guests.view"), CreatePermission("reservations.manage")]);

            var result = await _sut.UpdatePermissionsAsync(employee.Id,
                new UpdateEmployeePermissionsDto { Permissions = ["guests.view", "reservations.manage"] }, Guid.NewGuid());

            Assert.Equal(new[] { "guests.view", "reservations.manage" }, result.Permissions);
            Assert.False(result.IsOwner);
            _employees.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdatePermissionsAsync_MakeOwner_SetsOwner()
        {
            var employee = CreateEmployee();
            _repository.Setup(r => r.GetForUpdateAsync(employee.Id)).ReturnsAsync(employee);

            var result = await _sut.UpdatePermissionsAsync(employee.Id, new UpdateEmployeePermissionsDto { IsOwner = true }, Guid.NewGuid());

            Assert.True(employee.IsOwner);
            Assert.True(result.IsOwner);
            _repository.Verify(r => r.GetPermissionsAsync(It.IsAny<IEnumerable<string>>()), Times.Never);
        }

        [Fact]
        public async Task UpdatePermissionsAsync_EmptyList_RemovesAllPermissions()
        {
            var employee = CreateEmployee();
            employee.Permissions.Add(CreatePermission());
            _repository.Setup(r => r.GetForUpdateAsync(employee.Id)).ReturnsAsync(employee);

            await _sut.UpdatePermissionsAsync(employee.Id, new UpdateEmployeePermissionsDto(), Guid.NewGuid());

            Assert.Empty(employee.Permissions);
        }

        [Fact]
        public async Task UpdatePermissionsAsync_RemoveOwnOwnerRights_ThrowsArgumentException()
        {
            var employee = CreateEmployee();
            employee.IsOwner = true;

            await Assert.ThrowsAsync<ArgumentException>(() =>
                _sut.UpdatePermissionsAsync(employee.Id, new UpdateEmployeePermissionsDto { IsOwner = false }, employee.Id));

            _employees.Verify(r => r.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task UpdatePermissionsAsync_UnknownPermission_ThrowsArgumentException()
        {
            var employee = CreateEmployee();
            _repository.Setup(r => r.GetForUpdateAsync(employee.Id)).ReturnsAsync(employee);
            _repository.Setup(r => r.GetPermissionsAsync(It.IsAny<IEnumerable<string>>())).ReturnsAsync([]);

            await Assert.ThrowsAsync<ArgumentException>(() =>
                _sut.UpdatePermissionsAsync(employee.Id, new UpdateEmployeePermissionsDto { Permissions = ["bestaat.niet"] }, Guid.NewGuid()));

            _employees.Verify(r => r.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task UpdatePermissionsAsync_NotFound_ThrowsKeyNotFoundException()
        {
            _repository.Setup(r => r.GetForUpdateAsync(It.IsAny<Guid>())).ReturnsAsync((Employee?)null);

            await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                _sut.UpdatePermissionsAsync(Guid.NewGuid(), new UpdateEmployeePermissionsDto(), Guid.NewGuid()));
        }

        [Fact]
        public async Task GetPermissionsAsync_ReturnsAllPermissions()
        {
            _repository.Setup(r => r.GetAllPermissionsAsync()).ReturnsAsync([CreatePermission("guests.view"), CreatePermission("pool.capacity.manage")]);

            var result = await _sut.GetPermissionsAsync();

            Assert.Equal(new[] { "guests.view", "pool.capacity.manage" }, result.Select(p => p.Code));
        }
    }
}

using VakantiePark.Domain.Entities;

namespace VakantiePark.UnitTests.Domain
{
    public class EmployeeTests
    {
        private static Employee CreateEmployee(params Role[] roles) => new() { Roles = roles.ToList() };

        private static Role CreateRole(string code, params string[] permissions) => new()
        {
            Code = code,
            Permissions = permissions.Select(p => new Permission { Code = p }).ToList(),
        };

        [Fact]
        public void HasPermission_IsTrueForAPermissionOfOneOfItsRoles()
        {
            var employee = CreateEmployee(CreateRole("pool", "pool.capacity.manage"));

            Assert.True(employee.HasPermission("pool.capacity.manage"));
        }

        [Fact]
        public void HasPermission_IsFalseForAPermissionNoRoleGives()
        {
            var employee = CreateEmployee(CreateRole("reception", "guests.view"));

            Assert.False(employee.HasPermission("pool.capacity.manage"));
        }

        [Fact]
        public void HasPermission_IsFalseForAnInactiveEmployee()
        {
            var employee = CreateEmployee(CreateRole("pool", "pool.capacity.manage"));
            employee.IsActive = false;

            Assert.False(employee.HasPermission("pool.capacity.manage"));
        }

        [Fact]
        public void Permissions_CombinesRolesWithoutDuplicates()
        {
            var employee = CreateEmployee(
                CreateRole("reception", "guests.view", "reservations.manage"),
                CreateRole("pool", "pool.capacity.manage", "guests.view"));

            Assert.Equal(
                ["guests.view", "reservations.manage", "pool.capacity.manage"],
                employee.Permissions.Select(p => p.Code).ToArray());
        }
    }
}

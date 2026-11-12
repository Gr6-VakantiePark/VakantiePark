using VakantiePark.Domain.Entities;

namespace VakantiePark.UnitTests.Domain
{
    public class EmployeeTests
    {
        private static Employee CreateEmployee(params string[] permissions) => new()
        {
            Permissions = permissions.Select(p => new Permission { Code = p }).ToList(),
        };

        [Fact]
        public void HasPermission_IsTrueForAnAssignedPermission()
        {
            var employee = CreateEmployee("pool.capacity.manage");

            Assert.True(employee.HasPermission("pool.capacity.manage"));
        }

        [Fact]
        public void HasPermission_IsFalseForAPermissionThatIsNotAssigned()
        {
            var employee = CreateEmployee("guests.view");

            Assert.False(employee.HasPermission("pool.capacity.manage"));
        }

        [Fact]
        public void HasPermission_IsFalseForAnInactiveEmployee()
        {
            var employee = CreateEmployee("pool.capacity.manage");
            employee.IsActive = false;

            Assert.False(employee.HasPermission("pool.capacity.manage"));
        }

        [Fact]
        public void HasPermission_IsTrueForAnyPermissionWhenOwner()
        {
            var employee = CreateEmployee();
            employee.IsOwner = true;

            Assert.True(employee.HasPermission("pool.capacity.manage"));
        }

        [Fact]
        public void HasPermission_IsFalseForAnInactiveOwner()
        {
            var employee = CreateEmployee();
            employee.IsOwner = true;
            employee.IsActive = false;

            Assert.False(employee.HasPermission("pool.capacity.manage"));
        }
    }
}

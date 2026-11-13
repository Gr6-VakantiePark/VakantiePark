using Microsoft.EntityFrameworkCore;
using VakantiePark.Domain.Entities;
using VakantiePark.Infrastructure.Security;
using VakantiePark.Shared.Common;

namespace VakantiePark.Infrastructure.Data.Seeders
{
    public static class EmployeeSeeder
    {
        private sealed record EmployeeSeed(string Email, string Password, string FirstName, string LastName, bool IsOwner, string[] Permissions);

        private static readonly EmployeeSeed[] Employees =
        [
            new("beheerder@dropuut.example", "Beheerder-2027!", "Bas", "Beheerder", true, []),
            new("receptie@dropuut.example", "Receptie-2027!", "Rosa", "Receptie", false,
                [Permissions.ViewGuests, Permissions.ManageGuests, Permissions.ManageReservations]),
            new("zwembad@dropuut.example", "Zwembad-2027!", "Zeno", "Zwembad", false, [Permissions.ManagePoolCapacity]),
        ];

        public static void UseEmployeeSeeder(this DbContext context)
        {
            if (context.Set<Employee>().Any())
                return;

            var permissions = context.Set<Permission>().ToDictionary(p => p.Code);

            foreach (var seed in Employees)
            {
                var employee = new Employee
                {
                    Id = SeedIds.Employee(seed.Email),
                    Email = seed.Email,
                    PasswordHash = BCrypt.Net.BCrypt.EnhancedHashPassword(seed.Password, BCryptPasswordHasher.WorkFactor),
                    FirstName = seed.FirstName,
                    LastName = seed.LastName,
                    IsOwner = seed.IsOwner,
                    CreatedAt = DateTime.UtcNow,
                };

                foreach (string code in seed.Permissions)
                {
                    if (!permissions.TryGetValue(code, out var permission))
                        throw new InvalidOperationException($"Onbekend recht '{code}' bij medewerker '{seed.Email}'.");
                    employee.Permissions.Add(permission);
                }

                context.Add(employee);
            }

            context.SaveChanges();
        }
    }
}

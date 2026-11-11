using Microsoft.EntityFrameworkCore;
using VakantiePark.Domain.Entities;

namespace VakantiePark.Infrastructure.Data.Seeders
{
    public static class EmployeeSeeder
    {
        private sealed record EmployeeSeed(string Email, string Password, string FirstName, string LastName, string Role);

        private static readonly EmployeeSeed[] Employees =
        [
            new("beheerder@dropuut.example", "Beheerder-2027!", "Bas", "Beheerder", "administrator"),
            new("receptie@dropuut.example", "Receptie-2027!", "Rosa", "Receptie", "reception"),
            new("zwembad@dropuut.example", "Zwembad-2027!", "Zeno", "Zwembad", "pool"),
        ];

        public static void UseEmployeeSeeder(this DbContext context)
        {
            if (context.Set<Employee>().Any())
                return;

            var roles = context.Set<Role>().ToDictionary(r => r.Code);

            foreach (var seed in Employees)
            {
                if (!roles.TryGetValue(seed.Role, out var role))
                    throw new InvalidOperationException($"Onbekende rol '{seed.Role}' bij medewerker '{seed.Email}'.");

                context.Add(new Employee
                {
                    Id = Guid.NewGuid(),
                    Email = seed.Email,
                    PasswordHash = BCrypt.Net.BCrypt.EnhancedHashPassword(seed.Password, 12),
                    FirstName = seed.FirstName,
                    LastName = seed.LastName,
                    CreatedAt = DateTime.UtcNow,
                    Roles = [role],
                });
            }

            context.SaveChanges();
        }
    }
}

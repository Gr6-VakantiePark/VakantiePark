using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using VakantiePark.Domain.Entities;

namespace VakantiePark.Infrastructure.Data.Seeders
{
    public static class RoleSeeder
    {
        private static readonly string SeedFile = Path.Combine(AppContext.BaseDirectory, "Data", "Seed", "roles.json");

        private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNameCaseInsensitive = true };

        private sealed record RoleSeed(string Code, string Name, string? Description, List<string> Permissions);

        private sealed record RolesFile(Dictionary<string, string> Permissions, List<RoleSeed> Roles);

        public static void UseRoleSeeder(this DbContext context)
        {
            if (context.Set<Role>().Any())
                return;

            var file = JsonSerializer.Deserialize<RolesFile>(File.ReadAllText(SeedFile), JsonOptions)
                ?? throw new InvalidOperationException("roles.json is leeg of ongeldig.");

            var permissions = context.Set<Permission>().ToDictionary(p => p.Code);
            foreach (var (code, name) in file.Permissions)
            {
                if (permissions.ContainsKey(code))
                    continue;

                var permission = new Permission { Code = code, Name = name };
                context.Add(permission);
                permissions[code] = permission;
            }

            foreach (var seed in file.Roles)
            {
                var role = new Role { Code = seed.Code, Name = seed.Name, Description = seed.Description };

                foreach (string code in seed.Permissions)
                {
                    if (!permissions.TryGetValue(code, out var permission))
                        throw new InvalidOperationException($"Onbekende bevoegdheid '{code}' bij rol '{seed.Code}' in roles.json.");
                    role.Permissions.Add(permission);
                }

                context.Add(role);
            }

            context.SaveChanges();
        }
    }
}

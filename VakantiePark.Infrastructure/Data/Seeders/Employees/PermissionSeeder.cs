using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using VakantiePark.Domain.Entities;

namespace VakantiePark.Infrastructure.Data.Seeders
{
    public static class PermissionSeeder
    {
        private static readonly string SeedFile = Path.Combine(AppContext.BaseDirectory, "Data", "Seed", "permissions.json");

        private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNameCaseInsensitive = true };

        public static void UsePermissionSeeder(this DbContext context)
        {
            var seeds = JsonSerializer.Deserialize<Dictionary<string, string>>(File.ReadAllText(SeedFile), JsonOptions)
                ?? throw new InvalidOperationException("permissions.json is leeg of ongeldig.");

            var existing = context.Set<Permission>().Select(p => p.Code).ToHashSet();
            bool added = false;

            foreach (var (code, name) in seeds)
            {
                if (existing.Contains(code))
                    continue;

                context.Add(new Permission { Id = SeedIds.Permission(code), Code = code, Name = name });
                added = true;
            }

            if (added)
                context.SaveChanges();
        }
    }
}

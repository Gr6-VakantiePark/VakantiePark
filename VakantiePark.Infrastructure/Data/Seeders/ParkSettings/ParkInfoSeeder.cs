using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using VakantiePark.Domain.Entities;
using VakantiePark.Domain.Enums;

namespace VakantiePark.Infrastructure.Data.Seeders
{
    public static class ParkInfoSeeder
    {
        private static readonly string SeedFile = Path.Combine(AppContext.BaseDirectory, "Data", "Seed", "park-info.json");

        public static void UseParkInfoSeeder(this DbContext context)
        {
            using var doc = JsonDocument.Parse(File.ReadAllText(SeedFile));
            var root = doc.RootElement;

            SeedContact(context, root.GetProperty("contact"));
            SeedOpeningHours(context, root.GetProperty("openingHours"));
            SeedPoolBasins(context, root.GetProperty("poolBasins"));

            context.SaveChanges();
        }

        private static void SeedContact(DbContext context, JsonElement contact)
        {
            var settings = context.Set<ParkSettings>().OrderBy(s => s.Id).FirstOrDefault();
            if (settings == null)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(settings.Street))
            {
                settings.Street = contact.GetProperty("street").GetString()!;
            }

            if (string.IsNullOrWhiteSpace(settings.PostalCode))
            {
                settings.PostalCode = contact.GetProperty("postalCode").GetString()!;
            }

            if (string.IsNullOrWhiteSpace(settings.City))
            {
                settings.City = contact.GetProperty("city").GetString()!;
            }

            if (string.IsNullOrWhiteSpace(settings.PhoneNumber))
            {
                settings.PhoneNumber = contact.GetProperty("phoneNumber").GetString()!;
            }

            if (string.IsNullOrWhiteSpace(settings.Email))
            {
                settings.Email = contact.GetProperty("email").GetString()!;
            }
        }

        private static void SeedOpeningHours(DbContext context, JsonElement openingHours)
        {
            if (context.Set<OpeningHour>().Any())
            {
                return;
            }

            var sortOrder = 0;
            foreach (var item in openingHours.EnumerateArray())
            {
                var facilityName = item.GetProperty("facility").GetString();
                if (!Enum.TryParse<Facility>(facilityName, true, out var facility))
                {
                    throw new InvalidOperationException($"Onbekende voorziening '{facilityName}' in park-info.json.");
                }

                var label = item.GetProperty("label").GetString()!;
                context.Add(new OpeningHour
                {
                    Id = SeedIds.OpeningHour(facility.ToString(), label),
                    Facility = facility,
                    Label = label,
                    Value = item.GetProperty("value").GetString()!,
                    ShowOnHome = item.GetProperty("showOnHome").GetBoolean(),
                    SortOrder = sortOrder,
                });
                sortOrder++;
            }
        }

        private static void SeedPoolBasins(DbContext context, JsonElement poolBasins)
        {
            if (context.Set<PoolBasin>().Any())
            {
                return;
            }

            var sortOrder = 0;
            foreach (var item in poolBasins.EnumerateArray())
            {
                var name = item.GetProperty("name").GetString()!;
                context.Add(new PoolBasin
                {
                    Id = SeedIds.PoolBasin(name),
                    Name = name,
                    Description = item.GetProperty("description").GetString()!,
                    Size = item.GetProperty("size").GetString()!,
                    SortOrder = sortOrder,
                });
                sortOrder++;
            }
        }
    }
}

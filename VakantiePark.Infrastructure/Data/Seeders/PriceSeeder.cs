using Microsoft.EntityFrameworkCore;
using VakantiePark.Domain.Entities;

namespace VakantiePark.Infrastructure.Data.Seeders
{
    public static class PriceSeeder
    {
        private static readonly Dictionary<string, decimal> ExamplePricesPerType = new()
        {
            ["berk-4"] = 85m,
            ["berk-6"] = 105m,
            ["eik-6"] = 120m,
            ["eik-luxe-6"] = 155m,
            ["beuk-8"] = 145m,
            ["wilg-4-mindervalide"] = 90m,
            ["bosvilla-10"] = 240m,
            ["tentplaats-basis"] = 25m,
            ["tentplaats-comfort"] = 32m,
            ["caravanplaats"] = 35m,
            ["camperplaats"] = 35m,
            ["camperplaats-xl"] = 42m,
        };

        public static void UsePriceSeeder(this DbContext context)
        {
            if (!context.Set<ParkSettings>().Any())
            {
                context.Add(new ParkSettings
                {
                    CleaningFee = 75m,
                    TouristTaxPerPersonPerNight = 2.50m,
                    ArrivalTime = new TimeOnly(15, 0),
                    DepartureTime = new TimeOnly(10, 0),
                });
            }

            var typesWithoutPrice = context.Set<AccommodatieType>()
                .Where(t => t.PrijsPerNacht == 0m)
                .ToList();

            foreach (var type in typesWithoutPrice)
            {
                if (ExamplePricesPerType.TryGetValue(type.Slug, out var price))
                    type.PrijsPerNacht = price;
            }

            context.SaveChanges();
        }
    }
}

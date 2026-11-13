using Microsoft.EntityFrameworkCore;
using VakantiePark.Domain.Entities;

namespace VakantiePark.Infrastructure.Data.Seeders
{
    public static class PriceSeeder
    {
        private static readonly Dictionary<string, decimal> PricesPerType = new()
        {
            ["berk-4"] = 100m,
            ["berk-6"] = 125m,
            ["eik-6"] = 175m,
            ["eik-luxe-6"] = 200m,
            ["beuk-8"] = 175m,
            ["wilg-4-mindervalide"] = 100m,
            ["bosvilla-10"] = 250m,
            ["tentplaats-basis"] = 25m,
            ["tentplaats-comfort"] = 32m,
            ["caravanplaats"] = 35m,
            ["camperplaats"] = 35m,
            ["camperplaats-xl"] = 42m,
        };

        private static readonly Dictionary<string, decimal> OldExamplePricesPerType = new()
        {
            ["berk-4"] = 85m,
            ["berk-6"] = 105m,
            ["eik-6"] = 120m,
            ["eik-luxe-6"] = 155m,
            ["beuk-8"] = 145m,
            ["wilg-4-mindervalide"] = 90m,
            ["bosvilla-10"] = 240m,
        };

        public static void UsePriceSeeder(this DbContext context)
        {
            if (!context.Set<ParkSettings>().Any())
            {
                context.Add(new ParkSettings
                {
                    Id = SeedIds.ParkSettings,
                    CleaningFee = 75m,
                    TouristTaxPerPersonPerNight = 2.50m,
                    TouristTaxMinimumAge = 3,
                    WeekendSurchargePercentage = 10m,
                    PetFeePerNight = 10m,
                    ArrivalTime = new TimeOnly(15, 0),
                    DepartureTime = new TimeOnly(10, 0),
                    MinimumNightsHighSeason = 7,
                });
            }

            var types = context.Set<AccommodationType>().ToList();

            foreach (var type in types)
            {
                if (!PricesPerType.TryGetValue(type.Slug, out var price))
                {
                    continue;
                }

                var hasNoPrice = type.PricePerNight == 0m;
                var hasOldExamplePrice = OldExamplePricesPerType.TryGetValue(type.Slug, out var oldPrice)
                    && type.PricePerNight == oldPrice;

                if (hasNoPrice || hasOldExamplePrice)
                {
                    type.PricePerNight = price;
                }
            }

            context.SaveChanges();
        }
    }
}

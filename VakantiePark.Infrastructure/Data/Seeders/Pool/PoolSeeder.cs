using Microsoft.EntityFrameworkCore;
using VakantiePark.Domain.Entities;

namespace VakantiePark.Infrastructure.Data.Seeders
{
    public static class PoolSeeder
    {
        public const int CapacityPerTimeSlot = 50;

        private static readonly TimeOnly WeekdayOpeningTime = new(9, 0);
        private static readonly TimeOnly WeekendOpeningTime = new(8, 0);
        private static readonly TimeOnly ClosingTime = new(21, 0);

        public static void UsePoolSeeder(this DbContext context)
        {
            if (context.Set<PoolTimeSlot>().Any())
                return;

            foreach (var day in Enum.GetValues<DayOfWeek>())
            {
                var openingTime = day is DayOfWeek.Saturday or DayOfWeek.Sunday
                    ? WeekendOpeningTime
                    : WeekdayOpeningTime;

                for (var startTime = openingTime; startTime < ClosingTime; startTime = startTime.AddHours(1))
                {
                    context.Add(new PoolTimeSlot
                    {
                        Id = SeedIds.PoolTimeSlot(day, startTime),
                        DayOfWeek = day,
                        StartTime = startTime,
                        EndTime = startTime.AddHours(1),
                        Capacity = CapacityPerTimeSlot,
                    });
                }
            }

            context.SaveChanges();
        }
    }
}

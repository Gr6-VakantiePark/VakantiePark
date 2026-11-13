using Microsoft.EntityFrameworkCore;
using VakantiePark.Domain.Entities;
using VakantiePark.Domain.Interfaces;
using VakantiePark.Infrastructure.Data;

namespace VakantiePark.Infrastructure.Repositories
{
    public class PoolRepository(ApplicationDBContext context) : IPoolRepository
    {
        public Task<List<PoolTimeSlot>> GetTimeSlotsAsync(DayOfWeek dayOfWeek) =>
            context.PoolTimeSlots
                .AsNoTracking()
                .Where(t => t.DayOfWeek == dayOfWeek)
                .OrderBy(t => t.StartTime)
                .ToListAsync();

        public Task<Dictionary<Guid, int>> GetReservedPlacesAsync(DateOnly date) =>
            context.PoolReservations
                .Where(r => r.Date == date)
                .GroupBy(r => r.PoolTimeSlotId)
                .Select(g => new { TimeSlotId = g.Key, Persons = g.Sum(r => r.Persons) })
                .ToDictionaryAsync(g => g.TimeSlotId, g => g.Persons);
    }
}

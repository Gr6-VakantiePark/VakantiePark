using VakantiePark.Application.Extensions.Pool;
using VakantiePark.Application.Interfaces;
using VakantiePark.Domain.Interfaces;
using VakantiePark.Shared.DTOs.Pool;

namespace VakantiePark.Application.Services
{
    public class PoolService(IPoolRepository repository) : IPoolService
    {
        public async Task<PoolDayDto> GetTimeSlotsAsync(DateOnly? date = null)
        {
            var today = DateOnly.FromDateTime(DateTime.Today);
            var day = date ?? today;

            if (day < today)
            {
                throw new ArgumentException("De datum mag niet in het verleden liggen.");
            }

            var timeSlots = await repository.GetTimeSlotsAsync(day.DayOfWeek);
            var reservedPlaces = await repository.GetReservedPlacesAsync(day);

            return new PoolDayDto
            {
                Date = day,
                TimeSlots = timeSlots
                    .Select(t => t.ToDto(reservedPlaces.GetValueOrDefault(t.Id)))
                    .ToList(),
            };
        }
    }
}

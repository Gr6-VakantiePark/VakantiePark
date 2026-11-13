using VakantiePark.Domain.Entities;
using VakantiePark.Shared.DTOs.Pool;

namespace VakantiePark.Application.Extensions.Pool
{
    public static class PoolMappings
    {
        public static PoolTimeSlotDto ToDto(this PoolTimeSlot timeSlot, int reservedPlaces)
        {
            var availablePlaces = timeSlot.AvailablePlaces(reservedPlaces);

            return new PoolTimeSlotDto
            {
                Id = timeSlot.Id,
                StartTime = timeSlot.StartTime,
                EndTime = timeSlot.EndTime,
                Capacity = timeSlot.Capacity,
                AvailablePlaces = availablePlaces,
                IsFull = availablePlaces == 0,
            };
        }
    }
}

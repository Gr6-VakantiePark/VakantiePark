using VakantiePark.Domain.Entities;

namespace VakantiePark.Domain.Interfaces
{
    public interface IPoolRepository
    {
        Task<List<PoolTimeSlot>> GetTimeSlotsAsync(DayOfWeek dayOfWeek);

        Task<Dictionary<Guid, int>> GetReservedPlacesAsync(DateOnly date);
    }
}

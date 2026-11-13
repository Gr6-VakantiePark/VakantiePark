using VakantiePark.Shared.DTOs.Pool;

namespace VakantiePark.Application.Interfaces
{
    public interface IPoolService
    {
        Task<PoolDayDto> GetTimeSlotsAsync(DateOnly? date = null);
    }
}

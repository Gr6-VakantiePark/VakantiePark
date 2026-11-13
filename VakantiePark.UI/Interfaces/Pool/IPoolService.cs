using VakantiePark.Shared.Common;
using VakantiePark.Shared.DTOs.Pool;

namespace VakantiePark.UI.Interfaces
{
    public interface IPoolService
    {
        Task<ApiResponse<PoolDayDto>> GetTimeSlotsAsync(DateOnly date);
    }
}

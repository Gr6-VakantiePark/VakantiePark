using VakantiePark.Shared.Common;
using VakantiePark.Shared.DTOs.Park;

namespace VakantiePark.UI.Interfaces
{
    public interface IParkService
    {
        Task<ApiResponse<ParkInfoDto>> GetInfoAsync();
    }
}

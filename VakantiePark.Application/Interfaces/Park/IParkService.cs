using VakantiePark.Shared.DTOs.Park;

namespace VakantiePark.Application.Interfaces
{
    public interface IParkService
    {
        Task<ParkInfoDto> GetInfoAsync();
    }
}

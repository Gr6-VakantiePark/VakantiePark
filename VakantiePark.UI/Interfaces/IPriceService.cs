using VakantiePark.Shared.Common;
using VakantiePark.Shared.DTOs.Prices;
using VakantiePark.UI.Models;

namespace VakantiePark.UI.Interfaces
{
    public interface IPriceService
    {
        Task<ApiResponse<PriceDto>> GetPriceAsync(BookingRequest request);
    }
}

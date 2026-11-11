using VakantiePark.Shared.Common;
using VakantiePark.Shared.DTOs.Accommodations;
using VakantiePark.UI.Models;

namespace VakantiePark.UI.Interfaces
{
    public interface IBookingService
    {
        Task<ApiResponse<AccommodationDetailDto>> GetStayAsync(BookingRequest request);
    }
}

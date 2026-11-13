using VakantiePark.Shared.Common;
using VakantiePark.Shared.DTOs.Accommodations;
using VakantiePark.Shared.DTOs.Reservations;
using VakantiePark.UI.Models;

namespace VakantiePark.UI.Interfaces
{
    public interface IBookingService
    {
        Task<ApiResponse<AccommodationDetailDto>> GetStayAsync(BookingRequest request);

        Task<ApiResponse<bool>> CheckAvailabilityAsync(BookingRequest request);

        Task<ApiResponse<BookingDto>> CreateAsync(BookingRequest request, GuestDto guest, decimal expectedTotalPrice);
    }
}

using VakantiePark.Shared.DTOs.Reservations;

namespace VakantiePark.Application.Interfaces.Booking
{
    public interface IBookingService
    {
        Task<BookingDto> CreateAsync(CreateBookingDto request);
    }
}

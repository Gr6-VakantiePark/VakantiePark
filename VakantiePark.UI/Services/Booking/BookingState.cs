using VakantiePark.Shared.DTOs.Accommodations;
using VakantiePark.Shared.DTOs.Reservations;
using VakantiePark.UI.Models;

namespace VakantiePark.UI.Services;

public sealed class BookingState
{
    public ConfirmedBooking? LastBooking { get; private set; }

    public void Confirm(BookingDto booking, BookingRequest request, AccommodationDetailDto accommodation)
    {
        LastBooking = new ConfirmedBooking(booking, request, accommodation);
    }
}

public sealed record ConfirmedBooking(BookingDto Booking, BookingRequest Request, AccommodationDetailDto Accommodation);

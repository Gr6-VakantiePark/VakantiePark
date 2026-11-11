using VakantiePark.Shared.DTOs.Accommodations;
using VakantiePark.UI.Models;

namespace VakantiePark.UI.Services;

public sealed class BookingState
{
    public ConfirmedBooking? LastBooking { get; private set; }

    public void Confirm(BookingRequest request, AccommodationDetailDto accommodation, BookingDetails guest)
    {
        LastBooking = new ConfirmedBooking(request, accommodation, guest);
    }
}

public sealed record ConfirmedBooking(BookingRequest Request, AccommodationDetailDto Accommodation, BookingDetails Guest);

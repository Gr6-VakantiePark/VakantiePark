using VakantiePark.Shared.DTOs.Prices;
using VakantiePark.UI.Models;

namespace VakantiePark.UI.Services;

public sealed class BookingState
{
    public ConfirmedBooking? LastBooking { get; private set; }

    public void Confirm(BookingRequest request, BookingDetails guest, PriceDto? price)
    {
        LastBooking = new ConfirmedBooking(request, guest, price);
    }
}

public sealed record ConfirmedBooking(BookingRequest Request, BookingDetails Guest, PriceDto? Price);

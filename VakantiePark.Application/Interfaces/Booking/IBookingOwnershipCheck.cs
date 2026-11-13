using VakantiePark.Domain.Entities;

namespace VakantiePark.Application.Interfaces.Booking
{
    public interface IBookingOwnershipCheck
    {
        Task<Reservation> GetOwnedReservationAsync(string bookingNumber, string email);
    }
}

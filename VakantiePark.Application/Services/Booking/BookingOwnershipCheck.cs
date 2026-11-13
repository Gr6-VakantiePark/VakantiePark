using VakantiePark.Application.Extensions;
using VakantiePark.Application.Interfaces.Booking;
using VakantiePark.Domain.Entities;
using VakantiePark.Domain.Interfaces;

namespace VakantiePark.Application.Services.Booking
{
    public class BookingOwnershipCheck(IReservationRepository reservationRepository) : IBookingOwnershipCheck
    {
        public const string MissingDetails = "Vul je boekingsnummer en e-mailadres in.";
        public const string NotFound = "Er is geen boeking gevonden met dit boekingsnummer en e-mailadres.";

        public async Task<Reservation> GetOwnedReservationAsync(string bookingNumber, string email)
        {
            if (string.IsNullOrWhiteSpace(bookingNumber) || string.IsNullOrWhiteSpace(email))
                throw new ArgumentException(MissingDetails);

            var reservation = await reservationRepository.GetByBookingNumberAsync(bookingNumber.Trim());

            if (reservation is null || reservation.Guest.Email != EmployeeMappings.NormalizeEmail(email))
                throw new KeyNotFoundException(NotFound);

            return reservation;
        }
    }
}

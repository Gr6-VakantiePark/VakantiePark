using VakantiePark.Domain.Entities;
using VakantiePark.Shared.DTOs.Prices;
using VakantiePark.Shared.DTOs.Reservations;

namespace VakantiePark.Application.Extensions
{
    public static class BookingMappings
    {
        public static ReservationAvailabilityRequest ToAvailabilityRequest(this CreateBookingDto booking) =>
            new(booking.StartDate, booking.EndDate, booking.AccommodationId, booking.NumberOfGuests);

        public static Guest ToGuest(this GuestDto guest) => new()
        {
            Id = Guid.NewGuid(),
            Name = guest.Name.Trim(),
            Email = EmployeeMappings.NormalizeEmail(guest.Email),
            PhoneNumber = string.IsNullOrWhiteSpace(guest.PhoneNumber) ? null : guest.PhoneNumber.Trim(),
        };

        public static GuestDto ToDto(this Guest guest) => new()
        {
            Name = guest.Name,
            Email = guest.Email,
            PhoneNumber = guest.PhoneNumber,
        };

        public static BookingDto ToBookingDto(this Reservation reservation, string accommodationName, PriceDto price) => new()
        {
            Id = reservation.Id,
            BookingNumber = reservation.BookingNumber,
            AccommodationId = reservation.AccommodationId,
            AccommodationName = accommodationName,
            StartDate = reservation.StartDate,
            EndDate = reservation.EndDate,
            NumberOfGuests = reservation.NumberOfGuests,
            Status = reservation.Status.ToString(),
            Price = price,
            CreatedAt = reservation.CreatedAt,
            Guest = reservation.Guest.ToDto(),
        };
    }
}

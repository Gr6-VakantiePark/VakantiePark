using System.Security.Cryptography;
using VakantiePark.Application.Calculators;
using VakantiePark.Application.Extensions;
using VakantiePark.Application.Interfaces;
using VakantiePark.Application.Interfaces.Booking;
using VakantiePark.Domain.Entities;
using VakantiePark.Domain.Enums;
using VakantiePark.Domain.Interfaces;
using VakantiePark.Shared.DTOs.Reservations;

namespace VakantiePark.Application.Services.Booking
{
    public class BookingService(
        IAvailabilityService availabilityService,
        IAccommodationRepository accommodationRepository,
        IParkSettingsRepository settingsRepository,
        IReservationRepository reservationRepository,
        IRepository<Reservation> reservations,
        ITransactionRunner transactions) : IBookingService
    {
        public const string PriceChanged = "De prijs van dit verblijf is gewijzigd. Controleer de nieuwe prijs en bevestig je boeking opnieuw.";

        public async Task<BookingDto> CreateAsync(CreateBookingDto request)
        {
            request.Validate();

            return await transactions.InTransactionAsync(async () =>
            {
                await availabilityService.CheckForPeriodAvailability(request.ToAvailabilityRequest());

                var accommodation = await accommodationRepository.GetByIdAsync(request.AccommodationId)
                    ?? throw new KeyNotFoundException("De accommodatie is niet gevonden.");

                var settings = await settingsRepository.GetAsync()
                    ?? throw new KeyNotFoundException("De parkinstellingen zijn niet gevonden.");

                int nights = DateOnly.FromDateTime(request.EndDate).DayNumber - DateOnly.FromDateTime(request.StartDate).DayNumber;

                var price = PriceCalculator.Calculate(
                    accommodation.AccommodationType.PricePerNight,
                    nights,
                    request.NumberOfGuests,
                    settings.CleaningFee,
                    settings.TouristTaxPerPersonPerNight);

                if (price.Total != request.ExpectedTotalPrice)
                    throw new ArgumentException(PriceChanged);

                var reservation = new Reservation
                {
                    Id = Guid.NewGuid(),
                    BookingNumber = await CreateBookingNumberAsync(),
                    AccommodationId = accommodation.Id,
                    Guest = request.Guest.ToGuest(),
                    StartDate = request.StartDate,
                    EndDate = request.EndDate,
                    NumberOfGuests = request.NumberOfGuests,
                    Status = ReservationStatus.Confirmed,
                    TotalPrice = price.Total,
                    CreatedAt = DateTime.UtcNow,
                };

                reservations.Add(reservation);
                await reservations.SaveChangesAsync();

                return reservation.ToBookingDto(accommodation.Name, price);
            });
        }

        private async Task<string> CreateBookingNumberAsync()
        {
            string bookingNumber;
            do
            {
                bookingNumber = $"{DateTime.UtcNow:MM}{RandomNumberGenerator.GetInt32(1_000_000_000):D9}";
            }
            while (await reservationRepository.BookingNumberExistsAsync(bookingNumber));

            return bookingNumber;
        }
    }
}

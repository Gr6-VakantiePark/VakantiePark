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
        public const int FirstBookingSequence = 1001;
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

                if (request.Pets > accommodation.MaxPets)
                    throw new ArgumentException($"In {accommodation.Name} mogen maximaal {accommodation.MaxPets} huisdieren mee.");

                var price = PriceCalculator.Calculate(
                    accommodation.AccommodationType,
                    DateOnly.FromDateTime(request.StartDate),
                    DateOnly.FromDateTime(request.EndDate),
                    request.NumberOfGuests,
                    request.YoungChildren,
                    request.Pets,
                    settings);

                if (price.Total != request.ExpectedTotalPrice)
                    throw new ArgumentException(PriceChanged);

                DateTime now = DateTime.UtcNow;
                int sequence = (await reservationRepository.GetHighestBookingSequenceAsync() ?? FirstBookingSequence - 1) + 1;

                var reservation = new Reservation
                {
                    Id = Guid.NewGuid(),
                    BookingSequence = sequence,
                    BookingNumber = $"{now:MM}{sequence}",
                    AccommodationId = accommodation.Id,
                    Guest = request.Guest.ToGuest(),
                    StartDate = request.StartDate,
                    EndDate = request.EndDate,
                    NumberOfGuests = request.NumberOfGuests,
                    Status = request.HasPaid ? ReservationStatus.Confirmed : ReservationStatus.Pending,
                    TotalPrice = price.Total,
                    CreatedAt = now,
                };

                reservations.Add(reservation);
                await reservations.SaveChangesAsync();

                return reservation.ToBookingDto(accommodation.Name, price);
            });
        }
    }
}

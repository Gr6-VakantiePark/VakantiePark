using VakantiePark.Application.Extensions.Availability;
using VakantiePark.Application.Interfaces;
using VakantiePark.Domain.Entities;
using VakantiePark.Domain.Interfaces;
using VakantiePark.Shared.DTOs.Reservations;

namespace VakantiePark.Application.Services
{
    public class AvailabilityService(IReservationRepository reservationRepository,
                                    IUnrentablePeriodRepository unrentablePeriodRepository,
                                    IRepository<Accommodation> accommodationRepository) : IAvailabilityService
    {
        public async Task<bool> CheckForPeriodAvailability(ReservationAvailabilityRequest request)
        {
            request.Validate();

            Accommodation? accommodation = await accommodationRepository.GetAsync(request.AccommodationId);

            if (request.AccommodationId == Guid.Empty || accommodation == null)
                throw new KeyNotFoundException("De accommodatie-id is verplicht en moet een geldige accommodatie zijn.");

            if (request.AmountOfPeople <= 0 || request.AmountOfPeople > accommodation.MaxPersons)
                throw new ArgumentException("Het aantal personen moet groter zijn dan 0 en mag niet groter zijn dan de maximale capaciteit van de accommodatie.");

            List<Reservation> reservations = await reservationRepository.GetActiveAndFutureReservationsForAccomodation(request);
            List<UnrentablePeriod> unrentablePeriods = await unrentablePeriodRepository.GetActiveAndFuturePeriodsForAccomodation(request);

            bool overlapsReservation = reservations.Any();
            bool overlapsUnrentablePeriod = unrentablePeriods.Any();

            if (overlapsReservation || overlapsUnrentablePeriod)
                throw new ArgumentException("De accommodatie is niet beschikbaar in de opgegeven periode.");

            return true;
        }
    }
}

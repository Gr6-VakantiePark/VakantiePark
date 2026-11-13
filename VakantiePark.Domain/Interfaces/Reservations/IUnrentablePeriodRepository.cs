using VakantiePark.Domain.Entities;
using VakantiePark.Shared.DTOs.Reservations;

namespace VakantiePark.Domain.Interfaces
{
    public interface IUnrentablePeriodRepository
    {
        Task<List<UnrentablePeriod>> GetActiveAndFuturePeriodsForAccomodation(ReservationAvailabilityRequest request);
    }
}

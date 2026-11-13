using VakantiePark.Shared.DTOs.Reservations;

namespace VakantiePark.Application.Interfaces
{
    public interface IAvailabilityService
    {
        Task<bool> CheckForPeriodAvailability(ReservationAvailabilityRequest request);
    }
}

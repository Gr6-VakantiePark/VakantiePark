using System;

namespace VakantiePark.Shared.DTOs.Reservations
{
    public record ReservationAvailabilityRequest(DateTime StartTime, DateTime EndTime, Guid AccommodationId, int AmountOfPeople);
}

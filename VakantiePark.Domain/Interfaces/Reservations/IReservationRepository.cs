using System;
using VakantiePark.Domain.Entities;
using VakantiePark.Shared.DTOs.Reservations;

namespace VakantiePark.Domain.Interfaces
{
    public interface IReservationRepository
    {
        Task<List<Reservation>> GetActiveAndFutureReservationsForAccomodation(ReservationAvailabilityRequest request);
    }
}

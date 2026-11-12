using Microsoft.EntityFrameworkCore;
using VakantiePark.Domain.Entities;
using VakantiePark.Domain.Enums;
using VakantiePark.Domain.Interfaces;
using VakantiePark.Infrastructure.Data;
using VakantiePark.Shared.DTOs.Reservations;

namespace VakantiePark.Infrastructure.Repositories
{
    public class ReservationRepository(ApplicationDBContext context) : IReservationRepository
    {
        public async Task<List<Reservation>> GetActiveAndFutureReservationsForAccomodation(ReservationAvailabilityRequest filters)
            => await Overlaps(context.Reservations.Where(r => r.Status != ReservationStatus.Cancelled), filters).ToListAsync();

        public Task<bool> BookingNumberExistsAsync(string bookingNumber) =>
            context.Reservations.AnyAsync(r => r.BookingNumber == bookingNumber);

        private static IQueryable<Reservation> Overlaps(IQueryable<Reservation> reservations, ReservationAvailabilityRequest request)
             => reservations.Where(p => request.StartTime < p.EndDate && p.StartDate < request.EndTime && p.AccommodationId == request.AccommodationId);
    }
}

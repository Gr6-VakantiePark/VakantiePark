using Microsoft.EntityFrameworkCore;
using VakantiePark.Domain.Entities;
using VakantiePark.Domain.Interfaces;
using VakantiePark.Infrastructure.Data;
using VakantiePark.Shared.DTOs.Reservations;

namespace VakantiePark.Infrastructure.Repositories
{
    public class UnrentablePeriodRepository(ApplicationDBContext context) : IUnrentablePeriodRepository
    {
        public async Task<List<UnrentablePeriod>> GetActiveAndFuturePeriodsForAccomodation(ReservationAvailabilityRequest filters)
            => await Overlaps(context.UnrentablePeriods, filters).ToListAsync();

        private static IQueryable<UnrentablePeriod> Overlaps(IQueryable<UnrentablePeriod> unrentablePeriods, ReservationAvailabilityRequest request)
             => unrentablePeriods.Where(p => request.StartTime < p.EndDate && p.StartDate < request.EndTime && p.AccommodationId == request.AccommodationId);
    }
}
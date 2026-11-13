using Microsoft.EntityFrameworkCore;
using VakantiePark.Domain.Entities;
using VakantiePark.Domain.Interfaces;
using VakantiePark.Infrastructure.Data;

namespace VakantiePark.Infrastructure.Repositories
{
    public class ParkSettingsRepository(ApplicationDBContext context) : IParkSettingsRepository
    {
        public Task<ParkSettings?> GetAsync() =>
            context.ParkSettings
                .AsNoTracking()
                .OrderBy(i => i.Id)
                .FirstOrDefaultAsync();

        public Task<List<OpeningHour>> GetOpeningHoursAsync() =>
            context.OpeningHours
                .AsNoTracking()
                .OrderBy(o => o.SortOrder)
                .ToListAsync();

        public Task<List<PoolBasin>> GetPoolBasinsAsync() =>
            context.PoolBasins
                .AsNoTracking()
                .OrderBy(p => p.SortOrder)
                .ToListAsync();
    }
}

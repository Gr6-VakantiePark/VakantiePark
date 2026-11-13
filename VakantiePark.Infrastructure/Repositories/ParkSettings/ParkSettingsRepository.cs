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
    }
}

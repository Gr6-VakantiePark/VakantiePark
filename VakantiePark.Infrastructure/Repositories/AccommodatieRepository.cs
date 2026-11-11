using Microsoft.EntityFrameworkCore;
using VakantiePark.Application.Interfaces;
using VakantiePark.Domain.Entities;
using VakantiePark.Infrastructure.Data;

namespace VakantiePark.Infrastructure.Repositories
{
    public class AccommodatieRepository(ApplicationDBContext context) : IAccommodatieRepository
    {
        public Task<List<Accommodatie>> GetAllAsync() =>
            context.Accommodaties
                .AsNoTracking()
                .Include(a => a.AccommodatieType)
                .OrderBy(a => a.Code)
                .ToListAsync();

        public Task<Accommodatie?> GetByIdAsync(int id) =>
            context.Accommodaties
                .AsNoTracking()
                .Include(a => a.AccommodatieType)
                .Include(a => a.Voorzieningen)
                .FirstOrDefaultAsync(a => a.Id == id);

        public Task<AccommodatieType?> GetTypeBySlugAsync(string slug) =>
            context.AccommodatieTypes
                .AsNoTracking()
                .Include(t => t.Accommodaties)
                .FirstOrDefaultAsync(t => t.Slug == slug);
    }
}

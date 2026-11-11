using Microsoft.EntityFrameworkCore;
using VakantiePark.Application.Interfaces;
using VakantiePark.Domain.Entities;
using VakantiePark.Infrastructure.Data;

namespace VakantiePark.Infrastructure.Repositories
{
    public class AfbeeldingRepository(ApplicationDBContext context) : IAfbeeldingRepository
    {
        public Task<Afbeelding?> GetMetBestandAsync(int id) =>
            context.Afbeeldingen
                .AsNoTracking()
                .Include(a => a.Bestand)
                .FirstOrDefaultAsync(a => a.Id == id);
    }
}

using Microsoft.EntityFrameworkCore;
using VakantiePark.Application.Interfaces;
using VakantiePark.Domain.Entities;
using VakantiePark.Infrastructure.Data;

namespace VakantiePark.Infrastructure.Repositories
{
    public class PhotoRepository(ApplicationDBContext context) : IPhotoRepository
    {
        public Task<Photo?> GetWithFileAsync(int id) =>
            context.Photos
                .AsNoTracking()
                .Include(a => a.File)
                .FirstOrDefaultAsync(a => a.Id == id);
    }
}

using Microsoft.EntityFrameworkCore;
using VakantiePark.Domain.Interfaces;
using VakantiePark.Infrastructure.Data;

namespace VakantiePark.Infrastructure.Repositories
{
    public class EfRepository<T>(ApplicationDBContext context) : IRepository<T> where T : class
    {
        private readonly DbSet<T> _dbSet = context.Set<T>();
        public void Add(T entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            _dbSet.Add(entity);
        }

        public void Delete(T entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            _dbSet.Remove(entity);
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T?> GetAsync(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }

        public void Update(T entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            _dbSet.Update(entity);
        }
    }
}

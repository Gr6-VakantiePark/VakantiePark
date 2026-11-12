namespace VakantiePark.Domain.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<T?> GetAsync(Guid id);
        Task<List<T>> GetAllAsync();
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        Task SaveChangesAsync();
    }
}

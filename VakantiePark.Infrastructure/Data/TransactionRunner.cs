using VakantiePark.Domain.Interfaces;

namespace VakantiePark.Infrastructure.Data
{
    public class TransactionRunner(ApplicationDBContext context) : ITransactionRunner
    {
        public async Task<T> InTransactionAsync<T>(Func<Task<T>> work)
        {
            ArgumentNullException.ThrowIfNull(work);

            await using var transaction = await context.Database.BeginTransactionAsync();
            T result = await work();
            await transaction.CommitAsync();
            return result;
        }
    }
}

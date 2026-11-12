namespace VakantiePark.Domain.Interfaces
{
    public interface ITransactionRunner
    {
        Task<T> InTransactionAsync<T>(Func<Task<T>> work);
    }
}

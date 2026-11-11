using VakantiePark.Domain.Entities;

namespace VakantiePark.Application.Interfaces
{
    public interface IAfbeeldingRepository
    {
        Task<Afbeelding?> GetMetBestandAsync(int id);
    }
}

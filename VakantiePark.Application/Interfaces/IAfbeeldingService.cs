using VakantiePark.Shared.DTOs.Afbeeldingen;

namespace VakantiePark.Application.Interfaces
{
    public interface IAfbeeldingService
    {
        Task<AfbeeldingBestandDto> GetBestandAsync(int id);
    }
}

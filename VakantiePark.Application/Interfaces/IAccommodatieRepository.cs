using VakantiePark.Domain.Entities;
using VakantiePark.Domain.Enums;
using VakantiePark.Shared.DTOs.Accommodaties;

namespace VakantiePark.Application.Interfaces
{
    public interface IAccommodatieRepository
    {
        Task<List<Accommodatie>> GetAllAsync();

        Task<Accommodatie?> GetByIdAsync(int id);

        Task<List<AccommodatieType>> GetTypesAsync();

        Task<List<Voorziening>> GetVoorzieningenAsync();

        Task<Dictionary<int, List<AccommodatieCategorie>>> GetVoorzieningCategorieenAsync();

        Task<(int MaxPersonen, int MaxHuisdieren)> GetMaximaAsync();

        Task<(List<Accommodatie> Items, int TotalCount)> ZoekAsync(AccommodatieZoekCriteria pagination);
    }
}

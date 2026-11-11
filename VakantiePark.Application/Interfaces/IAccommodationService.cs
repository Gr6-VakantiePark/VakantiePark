using VakantiePark.Shared.Common;
using VakantiePark.Shared.DTOs.Accommodaties;

namespace VakantiePark.Application.Interfaces
{
    public interface IAccommodatieService
    {
        Task<List<AccommodatieDto>> GetAllAsync();

        Task<AccommodatieDetailDto> GetByIdAsync(int id);

        Task<ZoekOptiesDto> GetZoekOptiesAsync();

        Task<PaginatedResult<AccommodatieDetailDto>> ZoekAsync(AccommodatieZoekCriteria criteria);
    }
}

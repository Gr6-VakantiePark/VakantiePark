using VakantiePark.Shared.Common;
using VakantiePark.Shared.DTOs.Accommodaties;

namespace VakantiePark.UI.Interfaces
{
    public interface IZoekService
    {
        Task<ApiResponse<ZoekOptiesDto>> GetZoekOptiesAsync();

        Task<ApiResponse<PaginatedResult<AccommodatieDetailDto>>> ZoekAsync(AccommodatieZoekCriteria criteria);
    }
}

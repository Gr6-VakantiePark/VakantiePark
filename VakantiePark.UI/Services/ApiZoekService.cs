using VakantiePark.Shared.Common;
using VakantiePark.Shared.DTOs.Accommodaties;
using VakantiePark.UI.Interfaces;

namespace VakantiePark.UI.Services
{
    public class ApiZoekService(IHttpService http) : IZoekService
    {
        public Task<ApiResponse<ZoekOptiesDto>> GetZoekOptiesAsync() =>
            http.GetAsync<ZoekOptiesDto>("accommodaties/zoekopties");

        public Task<ApiResponse<PaginatedResult<AccommodatieDetailDto>>> ZoekAsync(AccommodatieZoekCriteria criteria) =>
            http.GetAsync<PaginatedResult<AccommodatieDetailDto>>($"accommodaties/zoek?{criteria.ToQueryString()}");
    }
}

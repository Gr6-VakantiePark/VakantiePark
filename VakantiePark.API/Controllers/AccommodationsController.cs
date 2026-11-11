using Microsoft.AspNetCore.Mvc;
using VakantiePark.Application.Interfaces;
using VakantiePark.Shared.Common;
using VakantiePark.Shared.DTOs.Accommodations;

namespace VakantiePark.API.Controllers
{
    [ApiController]
    [Route("accommodations")]
    public class AccommodationsController(IAccommodationService service) : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType<ApiResponse<List<AccommodationDto>>>(StatusCodes.Status200OK)]
        public Task<IActionResult> GetAll() =>
            this.RunAsync(() => service.GetAllAsync());

        [HttpGet("search-options")]
        [ProducesResponseType<ApiResponse<SearchOptionsDto>>(StatusCodes.Status200OK)]
        public Task<IActionResult> GetZoekOpties() =>
            this.RunAsync(() => service.GetSearchOptionsAsync());

        [HttpGet("search")]
        [ProducesResponseType<ApiResponse<PaginatedResult<AccommodationDetailDto>>>(StatusCodes.Status200OK)]
        [ProducesResponseType<ApiResponse<PaginatedResult<AccommodationDetailDto>>>(StatusCodes.Status400BadRequest)]
        public Task<IActionResult> Search([FromQuery] AccommodationSearchCriteria criteria) =>
            this.RunAsync(() => service.SearchAsync(criteria));

        [HttpGet("{id:int}")]
        [ProducesResponseType<ApiResponse<AccommodationDetailDto>>(StatusCodes.Status200OK)]
        [ProducesResponseType<ApiResponse<AccommodationDetailDto>>(StatusCodes.Status404NotFound)]
        public Task<IActionResult> GetById(int id) =>
            this.RunAsync(() => service.GetByIdAsync(id));
    }
}

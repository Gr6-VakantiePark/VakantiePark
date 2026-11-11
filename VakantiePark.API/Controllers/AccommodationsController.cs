using Microsoft.AspNetCore.Mvc;
using VakantiePark.Application.Interfaces;
using VakantiePark.Shared.Common;
using VakantiePark.Shared.DTOs.Accommodaties;

namespace VakantiePark.API.Controllers
{
    [ApiController]
    [Route("accommodaties")]
    public class AccommodatiesController(IAccommodatieService service) : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType<ApiResponse<List<AccommodatieDto>>>(StatusCodes.Status200OK)]
        public Task<IActionResult> GetAll() =>
            this.RunAsync(() => service.GetAllAsync());

        [HttpGet("zoekopties")]
        [ProducesResponseType<ApiResponse<ZoekOptiesDto>>(StatusCodes.Status200OK)]
        public Task<IActionResult> GetZoekOpties() =>
            this.RunAsync(() => service.GetZoekOptiesAsync());

        [HttpGet("zoek")]
        [ProducesResponseType<ApiResponse<PaginatedResult<AccommodatieDetailDto>>>(StatusCodes.Status200OK)]
        [ProducesResponseType<ApiResponse<PaginatedResult<AccommodatieDetailDto>>>(StatusCodes.Status400BadRequest)]
        public Task<IActionResult> Zoek([FromQuery] AccommodatieZoekCriteria criteria) =>
            this.RunAsync(() => service.ZoekAsync(criteria));

        [HttpGet("{id:int}")]
        [ProducesResponseType<ApiResponse<AccommodatieDetailDto>>(StatusCodes.Status200OK)]
        [ProducesResponseType<ApiResponse<AccommodatieDetailDto>>(StatusCodes.Status404NotFound)]
        public Task<IActionResult> GetById(int id) =>
            this.RunAsync(() => service.GetByIdAsync(id));
    }
}

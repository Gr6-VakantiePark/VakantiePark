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
        /// <summary>Alle accommodaties.</summary>
        [HttpGet]
        [ProducesResponseType<ApiResponse<List<AccommodatieDto>>>(StatusCodes.Status200OK)]
        public Task<IActionResult> GetAll() =>
            this.RunAsync(() => service.GetAllAsync());

        /// <summary>Eén accommodatie met alle details.</summary>
        [HttpGet("{id:int}")]
        [ProducesResponseType<ApiResponse<AccommodatieDetailDto>>(StatusCodes.Status200OK)]
        [ProducesResponseType<ApiResponse<AccommodatieDetailDto>>(StatusCodes.Status404NotFound)]
        public Task<IActionResult> GetById(int id) =>
            this.RunAsync(() => service.GetByIdAsync(id));
    }
}

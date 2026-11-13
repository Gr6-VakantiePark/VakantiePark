using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VakantiePark.Application.Interfaces;
using VakantiePark.Shared.Common;
using VakantiePark.Shared.DTOs.Pool;

namespace VakantiePark.API.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("pool")]
    public class PoolController(IPoolService poolService) : ControllerBase
    {
        [HttpGet("time-slots")]
        [ProducesResponseType<ApiResponse<PoolDayDto>>(StatusCodes.Status200OK)]
        [ProducesResponseType<ApiResponse<PoolDayDto>>(StatusCodes.Status400BadRequest)]
        public Task<IActionResult> GetTimeSlots([FromQuery] DateOnly? date) =>
            this.RunAsync(() => poolService.GetTimeSlotsAsync(date));
    }
}

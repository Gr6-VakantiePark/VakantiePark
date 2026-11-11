using Microsoft.AspNetCore.Mvc;
using VakantiePark.Application.Interfaces;
using VakantiePark.Shared.Common;

namespace VakantiePark.API.Controllers
{
    [ApiController]
    [Route("afbeeldingen")]
    public class AfbeeldingenController(IAfbeeldingService service) : ControllerBase
    {
        [HttpGet("{id:int}")]
        [ResponseCache(Duration = 86400)]
        [ProducesResponseType<FileContentResult>(StatusCodes.Status200OK)]
        [ProducesResponseType<ApiResponse<object>>(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var afbeelding = await service.GetBestandAsync(id);
                return File(afbeelding.Data, afbeelding.ContentType);
            }
            catch (KeyNotFoundException exception)
            {
                return NotFound(ApiResponse<object>.Fail(exception.Message));
            }
        }
    }
}

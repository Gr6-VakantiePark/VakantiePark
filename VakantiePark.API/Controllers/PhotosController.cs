using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VakantiePark.Application.Interfaces;
using VakantiePark.Shared.Common;

namespace VakantiePark.API.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("photos")]
    public class PhotosController(IPhotoService service) : ControllerBase
    {
        [HttpGet("{id:int}")]
        [ResponseCache(Duration = 86400)]
        [ProducesResponseType<FileContentResult>(StatusCodes.Status200OK)]
        [ProducesResponseType<ApiResponse<object>>(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var photo = await service.GetFileAsync(id);
                return File(photo.Data, photo.ContentType);
            }
            catch (KeyNotFoundException exception)
            {
                return NotFound(ApiResponse<object>.Fail(exception.Message));
            }
        }
    }
}

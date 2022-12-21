using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using Ads.Application.Common.Responces;
using Ads.Application.Upload.Commands;

namespace Ads.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : BaseController
    {

        [HttpPost, DisableRequestSizeLimit]
        public async Task<ActionResult<UploadResponseDto>> Upload(List<IFormFile> files)
        {
            var command = new UploadCommand()
            {
                files = files
            };
            
            var response = await Mediator.Send(command);
            
            return Ok(response);
        }
    }
}

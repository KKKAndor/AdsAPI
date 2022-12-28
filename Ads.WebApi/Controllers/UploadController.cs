using Microsoft.AspNetCore.Mvc;
using Ads.Application.Upload.Commands;

namespace Ads.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : BaseController
    {
        [HttpPost, DisableRequestSizeLimit]
        public async Task<ActionResult<string>> Upload(IFormFile file)
        {
            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                var content = stream.ToArray();
                
                var command = new UploadCommand()
                {
                    FileName = file.ContentDisposition,
                    FileContent = content
                };
                
                var response = await Mediator.Send(command);
            
                return Ok(response);
            }
        }
    }
}

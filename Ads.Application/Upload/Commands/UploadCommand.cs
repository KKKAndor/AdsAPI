using Ads.Application.Common.Responces;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Ads.Application.Upload.Commands;

public class UploadCommand: IRequest<UploadResponseDto>
{
    public List<IFormFile> files { get; set; }
}
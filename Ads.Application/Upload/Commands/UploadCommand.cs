using MediatR;

namespace Ads.Application.Upload.Commands;

public class UploadCommand: IRequest<string>
{
    public string FileName { get; set; }
    
    public byte[] FileContent { get; set; }
}
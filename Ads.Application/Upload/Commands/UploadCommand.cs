using System.ComponentModel.DataAnnotations;
using MediatR;

namespace Ads.Application.Upload.Commands;

public class UploadCommand: IRequest<string>
{
    [Required]
    public string FileName { get; set; }
    
    [Required]
    public byte[] FileContent { get; set; }
}
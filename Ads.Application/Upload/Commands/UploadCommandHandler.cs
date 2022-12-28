using System.Net.Http.Headers;
using Ads.Domain.Exceptions;
using MediatR;

namespace Ads.Application.Upload.Commands;

public class UploadCommandHandler 
    : IRequestHandler<UploadCommand, string>
{
    public async Task<string> Handle(UploadCommand request,
        CancellationToken cancellationToken)
    {      
        try
        {
            if (request.FileContent.Length <= 0 || string.IsNullOrWhiteSpace(request.FileName))
                throw new BadRequestException("Something went wrong during uploading file, try again");
            var folderName = Path.Combine("Resources", "Images");
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            var fileName = ContentDispositionHeaderValue.Parse(request.FileName).FileName.Trim('"');
            var fullPath = Path.Combine(pathToSave, fileName);
            var dbPath = Path.Combine(folderName, fileName);

            using (var writer = new BinaryWriter(File.OpenWrite(fullPath)))
            {
                writer.Write(request.FileContent);
            }
            
            return dbPath;
        }
        catch (Exception ex)
        {
            throw new ServerException(ex.Message);
        }
    }
}
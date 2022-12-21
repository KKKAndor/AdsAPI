using System.Net.Http.Headers;
using Ads.Application.Common.Responces;
using MediatR;

namespace Ads.Application.Upload.Commands;

public class UploadCommandHandler 
    : IRequestHandler<UploadCommand, UploadResponseDto>
{
    public async Task<UploadResponseDto> Handle(UploadCommand request,
        CancellationToken cancellationToken)
    {      
        long size = request.files.Sum(f => f.Length);

        List<string> dbPaths = new List<string>();
        
        foreach (var formFile in request.files)
        {
            try
            {
                var folderName = Path.Combine("Resources", "Images");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                if (formFile.Length > 0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(formFile.ContentDisposition).FileName.Trim('"');
                    var fullPath = Path.Combine(pathToSave, fileName);
                    var dbPath = Path.Combine(folderName, fileName);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                    }

                    dbPaths.Add(dbPath);
                }
            }
            catch (Exception ex)
            {
                return new UploadResponseDto() { Message = $"Something went wrong while uploading files. Error {ex.Message}",IsSuccessful = false};
            }
        }
        
        if(dbPaths.Count == 0)
            return new UploadResponseDto() { Message = "No files were uploaded", IsSuccessful = false};

        return new UploadResponseDto() { 
            Count = request.files.Count, 
            Size = size, 
            DbPaths = dbPaths, 
            IsSuccessful = true,
            Message = "You successfully uploaded files"
        };
    }
}
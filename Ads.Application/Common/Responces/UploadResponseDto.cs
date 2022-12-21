namespace Ads.Application.Common.Responces;

public class UploadResponseDto
{
    public int Count { get; set; }
    
    public long Size { get; set; }
    
    public List<string> DbPaths { get; set; }
    
    public bool IsSuccessful { get; set; }
    
    public string? Message { get; set; }
}
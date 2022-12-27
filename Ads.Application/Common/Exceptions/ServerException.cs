namespace Ads.Application.Common.Exceptions;

public class ServerException: Exception
{
    public ServerException(string Message)
        : base($"Something went wrong while uploading files. Error {Message}") { }
}
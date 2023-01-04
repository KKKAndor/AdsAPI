namespace Ads.Domain.Exceptions
{
    public class ServerException : Exception
    {
        public ServerException(string Message)
            : base($"Something went wrong on the server side. Error {Message}") { }
    }
}

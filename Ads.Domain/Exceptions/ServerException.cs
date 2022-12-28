using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ads.Domain.Exceptions
{
    public class ServerException : Exception
    {
        public ServerException(string Message)
            : base($"Something went wrong on the server side. Error {Message}") { }
    }
}

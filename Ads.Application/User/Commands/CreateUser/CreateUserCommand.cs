using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ads.Application.User.Commands.CreateUser
{
    public class CreateUserCommand : IRequest<Guid>
    {
        public string UserName { get; set; }

        public bool IsAdmin { get; set; }
    }
}

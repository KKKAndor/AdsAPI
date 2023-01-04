using MediatR;

namespace Ads.Application.User.Commands.CreateUser
{
    public class CreateUserCommand : IRequest<Guid>
    {
        public string UserName { get; set; }

        public bool IsAdmin { get; set; }
    }
}

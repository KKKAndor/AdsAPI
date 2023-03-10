using Ads.Domain.Models;
using MediatR;

namespace Ads.Application.User.Queries.GetUserList
{
    public class GetUserListQuery : IRequest<UserDataListVm>
    {
        public UserParameters userParameters { get; set; } = new();
    }
}

using Ads.Domain.Models;

namespace Ads.Application.User.Queries.GetUserList
{
    public class UserDataListVm
    {
        public PagedList<UserDataLookUpDto> UserList { get; set; }
    }
}

using MediatR;
using Ads.Domain.Interfaces;

namespace Ads.Application.User.Queries.GetUserList
{
    public class GetUserListQueryHandler
        : IRequestHandler<GetUserListQuery, UserDataListVm>
    {
        
        private readonly IUserRepository _repository;

        public GetUserListQueryHandler(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task<UserDataListVm> Handle(GetUserListQuery request,
            CancellationToken cancellationToken)
        {
            var pagedList = await _repository.GetAllUsersAsync<UserDataLookUpDto>(request.userParameters, cancellationToken);

            return new UserDataListVm { UserList = pagedList };
        }
    }
}

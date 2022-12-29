using AutoMapper;
using MediatR;
using Ads.Domain.Interfaces;

namespace Ads.Application.User.Queries.GetUserList
{
    public class GetUserListQueryHandler
        : IRequestHandler<GetUserListQuery, UserDataListVm>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetUserListQueryHandler(IUnitOfWork unitOfWork,
            IMapper mapper) => _unitOfWork = unitOfWork;

        public async Task<UserDataListVm> Handle(GetUserListQuery request,
            CancellationToken cancellationToken)
        {
            var pagedList = await _unitOfWork.Users.GetAllUsers<UserDataLookUpDto>(request.userParameters, cancellationToken);

            return new UserDataListVm { UserList = pagedList };
        }
    }
}

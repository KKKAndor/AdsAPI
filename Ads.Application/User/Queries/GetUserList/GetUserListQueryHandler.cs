using Ads.Application.Common;
using AutoMapper;
using MediatR;
using Ads.Domain.Entities;
using Ads.Domain.Interfaces;

namespace Ads.Application.User.Queries.GetUserList
{
    public class GetUserListQueryHandler
        : IRequestHandler<GetUserListQuery, UserDataListVm>
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IMapper _mapper;

        public GetUserListQueryHandler(IUnitOfWork unitOfWork,
            IMapper mapper) => (_unitOfWork, _mapper) = (unitOfWork, mapper);

        public async Task<UserDataListVm> Handle(GetUserListQuery request,
            CancellationToken cancellationToken)
        {
            var query = await _unitOfWork.Users.GetAllUsers(request.userParameters, cancellationToken);

            var pagedList = await PagedList<UserDataLookUpDto>
                .ToMappedPagedList<UserDataLookUpDto, AppUser>(
                    query,
                    request.userParameters.PageNumber,
                    request.userParameters.PageSize,
                    cancellationToken,
                    _mapper.ConfigurationProvider);

            return new UserDataListVm { UserList = pagedList };
        }
    }
}

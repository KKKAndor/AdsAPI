using Ads.Application.Common;
using Ads.Application.Common.Exceptions;
using Ads.Application.Interfaces;
using Ads.Application.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ads.Application.User.Queries.GetUserList
{
    public class GetUserListQueryHandler
        : IRequestHandler<GetUserListQuery, UserDataListVm>
    {
        private readonly IAdsDbContext _dbContext;

        private readonly IMapper _mapper;

        public GetUserListQueryHandler(IAdsDbContext dbContext,
            IMapper mapper) => (_dbContext, _mapper) = (dbContext, mapper);

        public async Task<UserDataListVm> Handle(GetUserListQuery request,
            CancellationToken cancellationToken)
        {
            var Query = await _dbContext.AppUsers
                .ProjectTo<UserDataLookUpDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            if (Query.Count == 0)
            {
                throw new NotFoundException(nameof(UserDataListVm), request.ToString());
            }

            Sort(ref Query, request.userParameters);

            if (!string.IsNullOrWhiteSpace(request.userParameters.ContainName))
                Search(ref Query, request.userParameters);

            var pagedList = new PagedList<UserDataLookUpDto>(
                Query,
                Query.Count(),
                request.userParameters.PageNumber,
                request.userParameters.PageSize
                );

            return new UserDataListVm { UserList = pagedList };
        }

        public void Search(ref List<UserDataLookUpDto> list, UserParameters userParameters)
        {
            list = list.Where(ad => ad.Name.ToLower()
                .Contains(userParameters.ContainName.ToLower())).ToList();
        }

        public void Sort(ref List<UserDataLookUpDto> list, UserParameters adsParameters)
        {
            var orders = adsParameters.OrderBy.Split(',');
            foreach (var order in orders)
            {
                switch (order)
                {
                    case "id":
                        list = list.OrderBy(o => o.Id).ToList();
                        break;
                    case "reverseid":
                        list = list.OrderBy(o => o.Id).Reverse().ToList();
                        break;
                    case "name":
                        list = list.OrderBy(o => o.Name).ToList();
                        break;
                    case "reverseName":
                        list = list.OrderBy(o => o.Name).Reverse().ToList();
                        break;
                    default:
                        list = list.OrderBy(o => o.Id).ToList();
                        break;
                }
            }
        }
    }
}

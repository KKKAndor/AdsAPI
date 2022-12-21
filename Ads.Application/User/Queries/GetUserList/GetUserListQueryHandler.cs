using Ads.Application.Common;
using Ads.Application.Common.Exceptions;
using Ads.Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Ads.Application.Common.Models;
using Ads.Domain;

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
            IQueryable<AppUser> query = _dbContext.AppUsers;
            
            ApplySearch(ref query, request.userParameters.Contain);
            
            ApplySort(ref query, request.userParameters.OrderBy);

            var list = query.ProjectTo<UserDataLookUpDto>(_mapper.ConfigurationProvider);
            
            var pagedList = await PagedList<UserDataLookUpDto>.ToPagedList(
                list,
                request.userParameters.PageNumber,
                request.userParameters.PageSize,
                cancellationToken
            );
            
            if (pagedList.TotalCount == 0)
            {
                throw new NotFoundException(nameof(UserDataListVm), request);
            }

            return new UserDataListVm { UserList = pagedList };
        }

        private void ApplySearch(ref IQueryable<AppUser> query, string? contain)
        {
            if(string.IsNullOrWhiteSpace(contain))
                return;
            query = query.Where(x => 
                x.Name.ToLower().Contains(contain.ToLower()));
        }

        private void ApplySort(ref IQueryable<AppUser> query, string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
            {
                query = query.OrderBy(x => x.Name);
            }
            var orderParams = orderByQueryString.Trim().Split(',');
            var propertyInfos = typeof(Ad).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var orderQueryBuilder = new StringBuilder();
            foreach (var param in orderParams)
            {
                if (string.IsNullOrWhiteSpace(param))
                    continue;
                var propertyFromQueryName = param.Split(" - ")[0];
                var objectProperty = propertyInfos.FirstOrDefault(pi => pi.Name.Equals(propertyFromQueryName, StringComparison.InvariantCultureIgnoreCase));
                if (objectProperty == null)
                    continue;
                var sortingOrder = param.EndsWith(" desc") ? "descending" : "ascending";
                switch (sortingOrder)
                {
                    case "descending":
                        switch (objectProperty.Name.ToString())
                        {
                            case "Name":
                                query = query.OrderBy(x => x.Name).Reverse();
                                break;
                        }
                        break;
                    case "ascending":
                        switch (objectProperty.Name.ToString())
                        {
                            case "Name":
                                query = query.OrderBy(x => x.Name);
                                break;
                        }
                        break;
                }
            }
        }
    }
}

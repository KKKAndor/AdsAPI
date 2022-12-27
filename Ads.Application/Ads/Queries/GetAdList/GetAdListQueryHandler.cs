using Ads.Application.Common;
using Ads.Application.Common.Exceptions;
using Ads.Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Text;
using Ads.Application.Common.Models;
using Ads.Domain.Entities;
using System.ComponentModel;

namespace Ads.Application.Ads.Queries.GetAdList
{
    public class GetAdListQueryHandler
        : IRequestHandler<GetAdListQuery, AdListVm>
    {
        private readonly IAdsDbContext _dbContext;

        private readonly IMapper _mapper;
        
        public GetAdListQueryHandler(IAdsDbContext dbContext,
            IMapper mapper) => (_dbContext, _mapper) = (dbContext, mapper);

        public async Task<AdListVm> Handle(GetAdListQuery request,
            CancellationToken cancellationToken)
        {
            IQueryable<Ad> query;

            var user = await _dbContext.AppUsers
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);
            
            query = _dbContext.Ads.AsNoTracking().Include(x=>x.User);
            if (user != null)
            {
                if (user.IsAdmin)
                {
                    query = query.IgnoreQueryFilters();
                }
            }

            ApplySearchFilter(ref query, request.AdsParameters);            
            
            ApplySort(ref query, request.AdsParameters.OrderBy);

            var pagedList = await PagedList<AdLookUpDto>
                .ToMappedPagedList<Ad, AdLookUpDto>(
                query,
                request.AdsParameters.PageNumber,
                request.AdsParameters.PageSize,
                cancellationToken,
                _mapper.ConfigurationProvider);

            return new AdListVm { Ads = pagedList };
        }

        private void ApplySearchFilter(ref IQueryable<Ad> query, AdsParameters? adsParameters)
        {
            if (adsParameters.MinRating != null)
                query = query.Where(x => x.Rating >= adsParameters.MinRating);
            if (adsParameters.MaxRating != null)
                query = query.Where(x => x.Rating <= adsParameters.MaxRating);
            if (adsParameters.MinCreationDate != null)
                query = query.Where(x => x.CreationDate >= adsParameters.MinCreationDate.Value);
            if (adsParameters.MaxCreationDate != null)
                query = query.Where(x => x.CreationDate <= adsParameters.MaxCreationDate.Value);
            if (!string.IsNullOrWhiteSpace(adsParameters.Contain))
            {
                query = query.Where(x =>
                    x.Description.ToLower().Contains(adsParameters.Contain.ToLower()) ||
                    x.Number.ToString().Contains(adsParameters.Contain.ToLower()) ||
                    x.User.UserName.Contains(adsParameters.Contain.ToLower()));
            }
        }


        private void ApplySort(ref IQueryable<Ad> query, string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
            {
                query = query.OrderBy(x => x.ExpirationDate);
                return;
            }
            var orderParams = orderByQueryString.Trim().Split(',');
            var propertyInfos = typeof(Ad).GetProperties(BindingFlags.Public | BindingFlags.Instance);
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
                            case "CreationDate":
                                query = query.OrderByDescending(x => x.CreationDate);
                                break;
                            case "ExpirationDate":
                                query = query.OrderByDescending(x => x.ExpirationDate);
                                break;
                            case "Number":
                                query = query.OrderByDescending(x => x.Number);
                                break;
                            case "Description":
                                query = query.OrderByDescending(x => x.Description);
                                break;
                            case "Rating":
                                query = query.OrderByDescending(x => x.Rating);
                                break;
                        }
                        break;
                    case "ascending":
                        switch (objectProperty.Name.ToString())
                        {
                            case "CreationDate":
                                query = query.OrderBy(x => x.CreationDate);
                                break;
                            case "ExpirationDate":
                                query = query.OrderBy(x => x.ExpirationDate);
                                break;
                            case "Number":
                                query = query.OrderBy(x => x.Number);
                                break;
                            case "Description":
                                query = query.OrderBy(x => x.Description);
                                break;
                            case "Rating":
                                query = query.OrderBy(x => x.Rating);
                                break;
                        }
                        break;
                }
            }
        }
    }
}

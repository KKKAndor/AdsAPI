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
            query = _dbContext.Ads.Include(x=>x.User);
            if (user != null)
            {
                if (user.IsAdmin)
                {
                    query = query.IgnoreQueryFilters();
                }
            }

            ApplyFilter(ref query, request.AdsParameters);
            
            ApplySearch(ref query, request.AdsParameters.Contain);
            
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

        private void ApplySearch(ref IQueryable<Ad> query, string? contain)
        {
            if(string.IsNullOrWhiteSpace(contain))
                return;
            query = query.Where(x => 
                x.Description.ToLower().Contains(contain.ToLower()) ||
                x.Number.ToString().Contains(contain.ToLower()) ||
                x.User.UserName.Contains(contain.ToLower()));
        }

        private void ApplyFilter(ref IQueryable<Ad> query, AdsParameters requestAdsParameters)
        {
            if(requestAdsParameters.MinRating != null)
                query = query.Where(x => x.Rating >= requestAdsParameters.MinRating);
            if(requestAdsParameters.MaxRating != null)
                query = query.Where(x => x.Rating <= requestAdsParameters.MaxRating);
            if(requestAdsParameters.MinCreationDate != null)
                query = query.Where(x => x.CreationDate >= requestAdsParameters.MinCreationDate.Value);
            if(requestAdsParameters.MaxCreationDate != null)
                query = query.Where(x => x.CreationDate <= requestAdsParameters.MaxCreationDate.Value);
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
                                query = query.OrderBy(x => x.CreationDate).Reverse();
                                break;
                            case "ExpirationDate":
                                query = query.OrderBy(x => x.ExpirationDate).Reverse();
                                break;
                            case "Number":
                                query = query.OrderBy(x => x.Number).Reverse();
                                break;
                            case "Description":
                                query = query.OrderBy(x => x.Description).Reverse();
                                break;
                            case "Rating":
                                query = query.OrderBy(x => x.Rating).Reverse();
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

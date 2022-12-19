using Ads.Application.Common;
using Ads.Application.Common.Exceptions;
using Ads.Application.Interfaces;
using Ads.Application.Models;
using Ads.Domain;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            List<AdLookUpDto> list = new List<AdLookUpDto>();
            var user = await _dbContext.AppUsers.FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);
            if (user != null)
            {
                if (user.IsAdmin)
                {
                    list = await _dbContext.Ads
                        .ProjectTo<AdLookUpDto>(_mapper.ConfigurationProvider)
                        .ToListAsync(cancellationToken);                    
                }
                else
                {
                    list = await _dbContext.Ads
                        .Where(ad => ad.ExpirationDate.Date > DateTime.Now.Date)
                        .ProjectTo<AdLookUpDto>(_mapper.ConfigurationProvider)
                        .ToListAsync(cancellationToken);
                }
            }
            else
            {
                list = await _dbContext.Ads
                                        .Where(ad => ad.ExpirationDate.Date > DateTime.Now.Date)
                                        .ProjectTo<AdLookUpDto>(_mapper.ConfigurationProvider)
                                        .ToListAsync(cancellationToken);
            }

            if (list.Count == 0)
            {
                throw new NotFoundException(nameof(AdListVm), request);
            }

            Filter(ref list, request.AdsParameters);

            if (!string.IsNullOrWhiteSpace(request.AdsParameters.ContainDescription))
                Search(ref list, request.AdsParameters);

            Sort(ref list, request.AdsParameters);

            var queryable = list.AsQueryable();

            var pagedList = PagedList<AdLookUpDto>.ToPagedList(
                queryable,
                request.AdsParameters.PageNumber,
                request.AdsParameters.PageSize
                );

            return new AdListVm { Ads = pagedList };
        }

        public void Filter(ref List<AdLookUpDto> list, AdsParameters adsParameters)
        {
            if (adsParameters.MinCreationDate != new DateTime())
                list = list.Where(ad => ad.CreationDate.Date >= adsParameters.MinCreationDate.Value.Date &&
                                    ad.CreationDate.TimeOfDay >= adsParameters.MinCreationDate.Value.TimeOfDay).ToList();

            if (adsParameters.MaxCreationDate != new DateTime())
                list = list.Where(ad => ad.CreationDate.Date <= adsParameters.MaxCreationDate.Value.Date &&
                                    ad.CreationDate.TimeOfDay <= adsParameters.MinCreationDate.Value.TimeOfDay).ToList();

            if (adsParameters.MinNumber != null)
                list = list.Where(ad => ad.Number >= adsParameters.MinNumber).ToList();

            if (adsParameters.MaxNumber != null)
                list = list.Where(ad => ad.Number <= adsParameters.MaxNumber).ToList();

            if (adsParameters.MinRating != null)
                list = list.Where(ad => ad.Rating >= adsParameters.MinRating).ToList();

            if (adsParameters.MaxRating != null)
                list = list.Where(ad => ad.Rating <= adsParameters.MaxRating).ToList();
        }

        public void Search(ref List<AdLookUpDto> list, AdsParameters adsParameters)
        {
            list = list.Where(ad => ad.Description.ToLower()
                .Contains(adsParameters.ContainDescription.ToLower())).ToList();
        }

        public void Sort(ref List<AdLookUpDto> list, AdsParameters adsParameters)
        {
            var orders = adsParameters.OrderBy.Split(',');
            foreach(var order in orders)
            {
                switch (order)
                {
                    case "number":
                        list = list.OrderBy(o => o.Number).ToList();
                        break;
                    case "reverseNumber":
                        list = list.OrderBy(o => o.Number).Reverse().ToList();
                        break;
                    case "rating":
                        list = list.OrderBy(o => o.Rating).ToList();
                        break;
                    case "reverseRating":
                        list = list.OrderBy(o => o.Rating).Reverse().ToList();
                        break;
                    case "creationDate":
                        list = list.OrderBy(o => o.CreationDate).ToList();
                        break;
                    case "reverseCreationDate":
                        list = list.OrderBy(o => o.CreationDate).Reverse().ToList();
                        break;
                    case "expirationDate":
                        list = list.OrderBy(o => o.ExpirationDate).ToList();
                        break;
                    case "reverseExpirationDate":
                        list = list.OrderBy(o => o.ExpirationDate).Reverse().ToList();
                        break;
                    default:
                        list = list.OrderBy(o => o.ExpirationDate).ToList();
                        break;
                }
            }
        }       
    }
}

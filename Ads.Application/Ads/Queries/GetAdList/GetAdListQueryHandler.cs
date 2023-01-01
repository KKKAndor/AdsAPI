using Ads.Application.Common;
using Ads.Domain.Entities;
using AutoMapper;
using MediatR;
using Ads.Domain.Interfaces;
using Ads.Domain.Models;
using AutoMapper.QueryableExtensions;

namespace Ads.Application.Ads.Queries.GetAdList
{
    public class GetAdListQueryHandler
        : IRequestHandler<GetAdListQuery, AdListVm>
    {
        private readonly IAdRepository _repository;

        public GetAdListQueryHandler(IAdRepository repository)
        {
            _repository = repository;
        }

        public async Task<AdListVm> Handle(GetAdListQuery request,
            CancellationToken cancellationToken)
        {
            var pagedList = await _repository.GetAllAds<AdLookUpDto>(request.AdsParameters, cancellationToken);

            return new AdListVm { Ads = pagedList };
        }
    }
}

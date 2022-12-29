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
        private readonly IUnitOfWork _unitOfWork;
        
        public GetAdListQueryHandler(IUnitOfWork unitOfWork,
            IMapper mapper) => _unitOfWork = unitOfWork;

        public async Task<AdListVm> Handle(GetAdListQuery request,
            CancellationToken cancellationToken)
        {
            var pagedList = await _unitOfWork.Ads.GetAllAds<AdLookUpDto>(request.AdsParameters, cancellationToken);

            return new AdListVm { Ads = pagedList };
        }
    }
}

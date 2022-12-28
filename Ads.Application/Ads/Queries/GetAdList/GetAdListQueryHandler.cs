using Ads.Application.Common;
using Ads.Domain.Entities;
using AutoMapper;
using MediatR;
using Ads.Domain.Interfaces;
using AutoMapper.QueryableExtensions;

namespace Ads.Application.Ads.Queries.GetAdList
{
    public class GetAdListQueryHandler
        : IRequestHandler<GetAdListQuery, AdListVm>
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IMapper _mapper;
        
        public GetAdListQueryHandler(IUnitOfWork unitOfWork,
            IMapper mapper) => (_unitOfWork, _mapper) = (unitOfWork, mapper);

        public async Task<AdListVm> Handle(GetAdListQuery request,
            CancellationToken cancellationToken)
        {
            var list = await _unitOfWork.Ads.GetAllAds(request.AdsParameters, cancellationToken);

            var pagedList = await PagedList<AdLookUpDto>.ToMappedPagedList<AdLookUpDto, Ad>(
                list,
                request.AdsParameters.PageNumber,
                request.AdsParameters.PageSize,
                cancellationToken,
                _mapper.ConfigurationProvider);

            return new AdListVm { Ads = pagedList };
        }
    }
}

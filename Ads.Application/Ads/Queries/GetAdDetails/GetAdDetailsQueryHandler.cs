using Ads.Domain.Interfaces;
using AutoMapper;
using MediatR;

namespace Ads.Application.Ads.Queries.GetAdDetails
{
    public class GetAdDetailsQueryHandler
        : IRequestHandler<GetAdDetailsQuery, AdDetailsVm>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAdDetailsQueryHandler(IUnitOfWork unitOfWork,
            IMapper mapper) => (_unitOfWork, _mapper) = (unitOfWork, mapper);

        public async Task<AdDetailsVm> Handle(GetAdDetailsQuery request,
            CancellationToken cancellationToken)
        {
            var entity = await _unitOfWork.Ads.GetAdById(request.Id, cancellationToken);

            return _mapper.Map<AdDetailsVm>(entity);
        }
    }
}

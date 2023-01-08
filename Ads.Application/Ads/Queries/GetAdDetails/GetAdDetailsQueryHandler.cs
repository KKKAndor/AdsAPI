using Ads.Domain.Interfaces;
using AutoMapper;
using MediatR;

namespace Ads.Application.Ads.Queries.GetAdDetails
{
    public class GetAdDetailsQueryHandler
        : IRequestHandler<GetAdDetailsQuery, AdDetailsVm>
    {
        private readonly IMapper _mapper;
        private readonly IAdRepository _repository;

        public GetAdDetailsQueryHandler(IAdRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<AdDetailsVm> Handle(GetAdDetailsQuery request,
            CancellationToken cancellationToken)
        {
            var entity = await _repository.GetAdByIdAsync(request.Id, request.UserId, cancellationToken);

            return _mapper.Map<AdDetailsVm>(entity);
        }
    }
}

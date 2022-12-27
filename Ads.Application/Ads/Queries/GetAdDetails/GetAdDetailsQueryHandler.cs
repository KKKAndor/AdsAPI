using Ads.Application.Common.Exceptions;
using Ads.Application.Interfaces;
using Ads.Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ads.Application.Ads.Queries.GetAdDetails
{
    public class GetAdDetailsQueryHandler
        : IRequestHandler<GetAdDetailsQuery, AdDetailsVm>
    {
        private readonly IAdsDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetAdDetailsQueryHandler(IAdsDbContext dbContext,
            IMapper mapper) => (_dbContext, _mapper) = (dbContext, mapper);

        public async Task<AdDetailsVm> Handle(GetAdDetailsQuery request,
            CancellationToken cancellationToken)
        {
            var entity = await _dbContext.Ads
                .AsNoTracking()
                .FirstOrDefaultAsync(a =>
                a.Id == request.Id && a.Deleted == false, cancellationToken);
            
            if (entity == null)        
            {
                throw new NotFoundException(nameof(Ad), request.Id);
            }
            
            return _mapper.Map<AdDetailsVm>(entity);
        }
    }
}

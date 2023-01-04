using Ads.Domain.Entities;
using Ads.Domain.Interfaces;
using MediatR;

namespace Ads.Application.Ads.Commands.CreateAd
{
    public class CreateAdCommandHandler
        : IRequestHandler<CreateAdCommand, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAdRepository _repository;

        public CreateAdCommandHandler(IUnitOfWork unitOfWork, IAdRepository repository)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
        }

        public async Task<Guid> Handle(CreateAdCommand request,
            CancellationToken cancellationToken)
        {
            var entity = Ad.Create(
                Guid.NewGuid(),
                request.UserId,
                request.Number,
                request.Description,
                request.ImagePath,
                request.Rating,
                request.ExpirationDate);
            
            await _repository.CreateAdAsync(entity, cancellationToken);
            
            await _unitOfWork.CompleteAsync(cancellationToken);

            return entity.Id;
        }
    }
}

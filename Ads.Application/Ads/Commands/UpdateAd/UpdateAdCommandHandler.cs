using Ads.Domain.Interfaces;
using MediatR;

namespace Ads.Application.Ads.Commands.UpdateAd
{
    public class UpdateAdCommandHandler
        : IRequestHandler<UpdateAdCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAdRepository _repository;

        public UpdateAdCommandHandler(IUnitOfWork unitOfWork, IAdRepository repository)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
        }

        public async Task<Unit> Handle(UpdateAdCommand request,
            CancellationToken cancellationToken)
        {
            await _repository.UpdateAdAsync(
                request.Id,
                request.UserId,
                request.Number,
                request.Description,
                request.ImagePath,
                request.Rating,
                request.ExpirationDate, 
                cancellationToken);

            await _unitOfWork.CompleteAsync(cancellationToken);

            return Unit.Value;
        }
    }
}

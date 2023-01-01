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
            var entity = await _repository.GetAdForUpdateAsync(request.UserId, request.Id, cancellationToken);

            entity.ExpirationDate = request.ExpirationDate;
            entity.Description = request.Description;
            entity.ImagePath = request.ImagePath;
            entity.Rating = request.Rating;
            entity.Number = request.Number;

            await _unitOfWork.CompleteAsync(cancellationToken);

            return Unit.Value;
        }
    }
}

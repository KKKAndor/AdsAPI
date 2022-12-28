using Ads.Domain.Interfaces;
using MediatR;

namespace Ads.Application.Ads.Commands.UpdateAd
{
    public class UpdateAdCommandHandler
        : IRequestHandler<UpdateAdCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateAdCommandHandler(IUnitOfWork unitOfWork) =>
            _unitOfWork = unitOfWork;

        public async Task<Unit> Handle(UpdateAdCommand request,
            CancellationToken cancellationToken)
        {
            var entity = await _unitOfWork.Ads.GetAdForUpdateAsync(request.UserId, request.Id, cancellationToken);

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

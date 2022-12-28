
using MediatR;
using Ads.Domain.Interfaces;

namespace Ads.Application.Ads.Commands.DeleteAd
{
    public class DeleteAdCommandHandler
        : IRequestHandler<DeleteAdCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteAdCommandHandler(IUnitOfWork unitOfWork) =>
            _unitOfWork = unitOfWork;

        public async Task<Unit> Handle(DeleteAdCommand request,
            CancellationToken cancellationToken)
        {
            await _unitOfWork.Ads.DeleteAdAsync(request.UserId,request.Id,cancellationToken);

            await _unitOfWork.CompleteAsync(cancellationToken);

            return Unit.Value;
        }
    }
}

using MediatR;
using Ads.Domain.Interfaces;

namespace Ads.Application.Ads.Commands.DeleteAd
{
    public class DeleteAdCommandHandler
        : IRequestHandler<DeleteAdCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAdRepository _repository;

        public DeleteAdCommandHandler(IUnitOfWork unitOfWork, IAdRepository repository)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
        }

        public async Task<Unit> Handle(DeleteAdCommand request,
            CancellationToken cancellationToken)
        {
            await _repository.DeleteAdAsync(request.Id, request.UserId, cancellationToken);

            await _unitOfWork.CompleteAsync(cancellationToken);

            return Unit.Value;
        }
    }
}

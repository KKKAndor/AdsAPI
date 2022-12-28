using Ads.Domain.Entities;
using Ads.Domain.Exceptions;
using Ads.Domain.Interfaces;
using MediatR;

namespace Ads.Application.Ads.Commands.CreateAd
{
    public class CreateAdCommandHandler
        : IRequestHandler<CreateAdCommand, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateAdCommandHandler(IUnitOfWork unitOfWork) =>
            _unitOfWork = unitOfWork;

        public async Task<Guid> Handle(CreateAdCommand request,
            CancellationToken cancellationToken)
        {
            var entity = new Ad
            {
                Id = Guid.NewGuid(),
                CreationDate = DateTime.Now,
                Description = request.Description,
                ExpirationDate = request.ExpirationDate,
                ImagePath = request.ImagePath,
                Number = request.Number,
                Rating = request.Rating,
                UserId = request.UserId,
                Deleted = false
            };
            
            await _unitOfWork.Ads.CreateAdAsync(request.UserId, entity, cancellationToken);
            
            await _unitOfWork.CompleteAsync(cancellationToken);

            return entity.Id;
        }
    }
}

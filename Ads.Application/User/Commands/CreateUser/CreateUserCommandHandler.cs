using Ads.Domain.Entities;
using Ads.Domain.Interfaces;
using MediatR;

namespace Ads.Application.User.Commands.CreateUser
{
    public class CreateUserCommandHandler
        : IRequestHandler<CreateUserCommand, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _repository;

        public CreateUserCommandHandler(IUnitOfWork unitOfWork, IUserRepository repository)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
        }

        public async Task<Guid> Handle(CreateUserCommand request,
            CancellationToken cancellationToken)
        {
            var entity = AppUser.Create(
                Guid.NewGuid(),
                request.UserName,
                request.IsAdmin);
            
            await _repository.CreateUserAsync(entity, cancellationToken);

            await _unitOfWork.CompleteAsync(cancellationToken);

            return entity.Id;
        }
    }
}

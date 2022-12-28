using Ads.Domain.Entities;
using Ads.Domain.Interfaces;
using MediatR;

namespace Ads.Application.User.Commands.CreateUser
{
    public class CreateUserCommandHandler
        : IRequestHandler<CreateUserCommand, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateUserCommandHandler(IUnitOfWork unitOfWork) =>
            _unitOfWork = unitOfWork;

        public async Task<Guid> Handle(CreateUserCommand request,
            CancellationToken cancellationToken)
        {
            var entity = new AppUser
            {
                Id = Guid.NewGuid(),
                IsAdmin = request.IsAdmin,
                UserName = request.UserName
            };
            
            await _unitOfWork.Users.CreateUserAsync(entity, cancellationToken);

            await _unitOfWork.CompleteAsync(cancellationToken);

            return entity.Id;
        }
    }
}

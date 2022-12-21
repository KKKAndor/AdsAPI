using Ads.Application.Interfaces;
using Ads.Domain;
using MediatR;

namespace Ads.Application.User.Commands.CreateUser
{
    public class CreateUserCommandHandler
        : IRequestHandler<CreateUserCommand, Guid>
    {
        private readonly IAdsDbContext _dbContext;

        public CreateUserCommandHandler(IAdsDbContext dbContext) =>
            _dbContext = dbContext;

        public async Task<Guid> Handle(CreateUserCommand request,
            CancellationToken cancellationToken)
        {
            var entity = new AppUser
            {
                Id = Guid.NewGuid(),
                IsAdmin = request.IsAdmin,
                UserName = request.UserName
            };

            await _dbContext.AppUsers.AddAsync(entity, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return entity.Id;
        }
    }
}

using Ads.Application.Common;
using Ads.Application.Common.Exceptions;
using Ads.Application.Common.Responces;
using Ads.Application.Interfaces;
using Ads.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ads.Application.Ads.Commands.UpdateAd
{
    public class UpdateAdCommandHandler
        : IRequestHandler<UpdateAdCommand>
    {
        private readonly IAdsDbContext _dbContext;

        public UpdateAdCommandHandler(IAdsDbContext dbContext) =>
            _dbContext = dbContext;

        public async Task<Unit> Handle(UpdateAdCommand request,
            CancellationToken cancellationToken)
        {
            var user = await 
                _dbContext.AppUsers.FirstOrDefaultAsync(
                    u => u.Id == request.UserId, cancellationToken);

            if (user == null)
            {
                throw new NotFoundException(nameof(AppUser), request.UserId);
            }

            var entity = await 
                _dbContext.Ads.FirstOrDefaultAsync(
                    a => a.Id == request.Id, cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Ad), request.Id);
            }

            if (!user.IsAdmin && entity.UserId != request.UserId)
                throw new BadRequestException("You cannot update this add");

            entity.ExpirationDate = request.ExpirationDate;
            entity.Description = request.Description;
            entity.ImagePath = request.ImagePath;
            entity.Rating = request.Rating;
            entity.Number = request.Number;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}

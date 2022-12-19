using Ads.Application.Common;
using Ads.Application.Common.Exceptions;
using Ads.Application.Interfaces;
using Ads.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ads.Application.Ads.Commands.UpdateAd
{
    public class UpdateAdCommandHandler
        : IRequestHandler<UpdateAdCommand, ResponceDto>
    {
        private readonly IAdsDbContext _dbContext;

        public UpdateAdCommandHandler(IAdsDbContext dbContext) =>
            _dbContext = dbContext;

        public async Task<ResponceDto> Handle(UpdateAdCommand request,
            CancellationToken cancellationToken)
        {
            var user = await _dbContext.AppUsers.FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

            var entity =
                await _dbContext.Ads.FirstOrDefaultAsync(a =>
                a.Id == request.Id && a.UserId == request.UserId, cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Ad), request.Id);
            }

            if (!user.IsAdmin && entity.UserId != request.UserId)
                return new ResponceDto { IsSuccessful = false, Message = "You cannot update this add" };

            entity.ExpirationDate = request.ExpirationDate;
            entity.Description = request.Description;
            entity.ImagePath = request.ImagePath;
            entity.Rating = request.Rating;
            entity.Number = request.Number;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return new ResponceDto { IsSuccessful = true, Message = "You updated Ad" };
        }
    }
}

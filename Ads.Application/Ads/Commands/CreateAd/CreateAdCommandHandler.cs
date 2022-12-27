using Ads.Application.Common;
using Ads.Application.Common.Exceptions;
using Ads.Application.Common.Responces;
using Ads.Application.Interfaces;
using Ads.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ads.Application.Ads.Commands.CreateAd
{
    public class CreateAdCommandHandler
        : IRequestHandler<CreateAdCommand, Guid>
    {
        private readonly IAdsDbContext _dbContext;

        public CreateAdCommandHandler(IAdsDbContext dbContext) =>
            _dbContext = dbContext;

        public async Task<Guid> Handle(CreateAdCommand request,
            CancellationToken cancellationToken)
        {            
            var user = await _dbContext.AppUsers.FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

            if (user == null)
            {
                throw new NotFoundException(nameof(AppUser), request.UserId);
            }
            
            var count = await _dbContext.Ads.Where(ad => ad.UserId == request.UserId).CountAsync(cancellationToken) + 1;

            if (count > 10 && !user.IsAdmin)
                throw new BadRequestException("You cannot create more than 10 Ads");

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

            await _dbContext.Ads.AddAsync(entity, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return entity.Id;
        }
    }
}

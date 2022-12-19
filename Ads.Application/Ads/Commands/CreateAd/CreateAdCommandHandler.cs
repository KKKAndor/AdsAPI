using Ads.Application.Common;
using Ads.Application.Interfaces;
using Ads.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ads.Application.Ads.Commands.CreateAd
{
    public class CreateAdCommandHandler
        : IRequestHandler<CreateAdCommand, ResponceDto>
    {
        private readonly IAdsDbContext _dbContext;

        public CreateAdCommandHandler(IAdsDbContext dbContext) =>
            _dbContext = dbContext;

        public async Task<ResponceDto> Handle(CreateAdCommand request,
            CancellationToken cancellationToken)
        {            
            var user = await _dbContext.AppUsers.FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

            if (_dbContext.Ads.Where(ad => ad.UserId == request.UserId).Count() + 1 > 10 && !user.IsAdmin)
                return new ResponceDto { IsSuccessful = false, Message = "User cannot create more than 10 Ads" };

            var entity = new Ad
            {
                Id = Guid.NewGuid(),
                CreationDate = DateTime.Now,
                Description = request.Description,
                ExpirationDate = request.ExpirationDate,
                ImagePath = request.ImagePath,
                Number = request.Number,
                Rating = request.Rating,
                UserId = request.UserId
            };

            await _dbContext.Ads.AddAsync(entity, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return new ResponceDto { IsSuccessful = true, Message = entity.Id.ToString() };
        }
    }
}

using Ads.Application.Common;
using Ads.Application.Common.Exceptions;
using Ads.Application.Interfaces;
using Ads.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ads.Application.Ads.Commands.DeleteAd
{
    public class DeleteAdCommandHandler
        : IRequestHandler<DeleteAdCommand, ResponceDto>
    {
        private readonly IAdsDbContext _dbContext;

        public DeleteAdCommandHandler(IAdsDbContext dbContext) =>
            _dbContext = dbContext;

        public async Task<ResponceDto> Handle(DeleteAdCommand request,
            CancellationToken cancellationToken)
        {
            var user = await _dbContext.AppUsers.FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

            if (user == null)
            {
                throw new NotFoundException(nameof(AppUser), request.UserId);
            }

            var entity = await _dbContext.Ads
                .FindAsync(new object[] { request.Id }, cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Ad), request.Id);
            }

            if (!user.IsAdmin && entity.UserId != request.UserId)
                throw new Exception("You cannot delete this add");

            _dbContext.Ads.Remove(entity);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return new ResponceDto { IsSuccessful = true, Message = "You deleted Ad" };
        }
    }
}

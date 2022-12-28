using Ads.Domain.Entities;
using Ads.Domain.Models;

namespace Ads.Domain.Interfaces;

public interface IUserRepository
{
    Task CreateUserAsync(AppUser user, CancellationToken cancellationToken);

    Task<IQueryable<AppUser>> GetAllUsers(UserParameters parameters, CancellationToken cancellationToken);
}
using Ads.Domain.Entities;
using Ads.Domain.Models;

namespace Ads.Domain.Interfaces;

public interface IUserRepository
{
    Task CreateUserAsync(AppUser user, CancellationToken cancellationToken);

    Task<AppUser> GetUserById(Guid UserId, CancellationToken cancellationToken);
    
    Task<PagedList<T>> GetAllUsers<T>(UserParameters parameters, CancellationToken cancellationToken);
}
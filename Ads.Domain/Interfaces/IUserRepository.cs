using Ads.Domain.Entities;
using Ads.Domain.Models;

namespace Ads.Domain.Interfaces;

public interface IUserRepository
{
    Task CreateUserAsync(AppUser user, CancellationToken cancellationToken);

    Task<AppUser> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken);
    
    Task<PagedList<T>> GetAllUsersAsync<T>(UserParameters parameters, CancellationToken cancellationToken);
}
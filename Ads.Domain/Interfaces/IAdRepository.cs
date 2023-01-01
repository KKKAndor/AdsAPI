using Ads.Domain.Entities;
using Ads.Domain.Models;

namespace Ads.Domain.Interfaces;

public interface IAdRepository
{
    Task CreateAdAsync(Guid UserId, Ad ad, CancellationToken cancellationToken);

    Task DeleteAdAsync(Guid UserId, Guid AdId, CancellationToken cancellationToken);

    Task<Ad> GetAdForUpdateAsync(Guid UserID, Guid AdId, CancellationToken cancellationToken);

    Task<Ad> GetAdById(Guid AdId, CancellationToken cancellationToken);

    Task<PagedList<T>> GetAllAds<T>(AdsParameters parameters, CancellationToken cancellationToken);
} 
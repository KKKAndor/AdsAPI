using Ads.Domain.Entities;
using Ads.Domain.Models;

namespace Ads.Domain.Interfaces;

public interface IAdRepository
{
    Task CreateAdAsync(Ad ad, CancellationToken cancellationToken);

    Task DeleteAdAsync(Guid UserId, Guid AdId, CancellationToken cancellationToken);

    Task UpdateAdAsync(
        Guid AdId, 
        Guid UserId, 
        int Number,
        string Description,
        string ImagePath,
        int Rating,
        DateTime ExpirationDate, CancellationToken cancellationToken);

    Task<Ad> GetAdById(Guid AdId, Guid UserId, CancellationToken cancellationToken);

    Task<PagedList<T>> GetAllAds<T>(AdsParameters parameters, CancellationToken cancellationToken);
} 
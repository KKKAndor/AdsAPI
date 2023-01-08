using Ads.Domain.Entities;
using Ads.Domain.Models;

namespace Ads.Domain.Interfaces;

public interface IAdRepository
{
    Task CreateAdAsync(Ad ad, CancellationToken cancellationToken);

    Task DeleteAdAsync(Guid adId, Guid userId, CancellationToken cancellationToken);

    Task UpdateAdAsync(
        Guid adId, 
        Guid userId, 
        int number,
        string description,
        string imagePath,
        int rating,
        DateTime expirationDate, CancellationToken cancellationToken);

    Task<Ad> GetAdByIdAsync(Guid adId, Guid userId, CancellationToken cancellationToken);

    Task<PagedList<T>> GetAllAdsAsync<T>(AdsParameters parameters, CancellationToken cancellationToken);
} 
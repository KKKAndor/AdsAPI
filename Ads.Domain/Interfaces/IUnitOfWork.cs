namespace Ads.Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IAdRepository Ads { get; }
    IUserRepository Users { get; }
    
    Task<int> CompleteAsync(CancellationToken cancellationToken);
}
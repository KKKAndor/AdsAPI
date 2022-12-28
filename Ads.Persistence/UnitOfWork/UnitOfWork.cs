using Ads.Domain.Interfaces;
using Ads.Persistence.Interfaces;
using Ads.Persistence.Repositories;

namespace Ads.Persistence.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly AdsDbContext _context;
    
    public UnitOfWork(AdsDbContext context)
    {
        _context = context;
        Ads = new AdRepository(_context);
        Users = new UserRepository(_context);
    }
    public IAdRepository Ads { get; private set; }
    public IUserRepository Users { get; private set; }

    public Task<int> CompleteAsync(CancellationToken cancellationToken)
    {
        return _context.SaveChangesAsync(cancellationToken);
    }
    public void Dispose()
    {
        _context.Dispose();
    }
}
using Ads.Domain.Interfaces;
using Ads.Domain.Primitives;
using Ads.Persistence.Repositories;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Ads.Persistence.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly AdsDbContext _context;
    
    public UnitOfWork(AdsDbContext context, IMapper mapper)
    {
        _context = context;
        Users = new UserRepository(_context, mapper);
        Ads = new AdRepository(_context, Users, mapper);
    }
    public IAdRepository Ads { get; private set; }
    public IUserRepository Users { get; private set; }

    public Task<int> CompleteAsync(CancellationToken cancellationToken)
    {
        UpdateAuditableEntities();
        
        return _context.SaveChangesAsync(cancellationToken);
    }
    public void Dispose()
    {
        _context.Dispose();
    }

    private void UpdateAuditableEntities()
    {
        IEnumerable<EntityEntry<IAuditableEntity>> entries =
            _context
                .ChangeTracker
                .Entries<IAuditableEntity>();

        foreach (var entityEntry in entries)
        {
            if (entityEntry.State == EntityState.Added)
            {
                entityEntry.Property(e => e.CreationDate).CurrentValue = DateTime.Now;
            }
        }
    }
}
using Ads.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Ads.Persistence.Interfaces;

public interface IAdsDbContext
{
    DbSet<Ad> Ads { get; set; }

    DbSet<AppUser> AppUsers { get; set; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
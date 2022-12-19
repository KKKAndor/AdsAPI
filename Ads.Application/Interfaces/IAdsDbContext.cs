﻿using Ads.Domain;
using Microsoft.EntityFrameworkCore;

namespace Ads.Application.Interfaces
{
    public interface IAdsDbContext
    {
        DbSet<Ad> Ads { get; set; }

        DbSet<AppUser> AppUsers { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}

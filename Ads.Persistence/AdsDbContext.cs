﻿using Ads.Application.Interfaces;
using Ads.Domain.Entities;
using Ads.Infrastructure.EntityTypeConfigurations;
using Microsoft.EntityFrameworkCore;

namespace Ads.Infrastructure
{
    public class AdsDbContext : DbContext, IAdsDbContext
    {
        public DbSet<Ad> Ads { get; set; }

        public DbSet<AppUser> AppUsers { get; set; }

        public AdsDbContext(DbContextOptions<AdsDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new AdConfiguration());
            builder.ApplyConfiguration(new UserConfiguration());
        }
    }

}

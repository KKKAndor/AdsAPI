using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ads.Domain.Entities;

namespace Ads.Infrastructure.EntityTypeConfigurations
{
    public class UserConfiguration : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.HasKey(ad => ad.Id);
            builder.HasIndex(ad => ad.Id).IsUnique();
            builder.HasIndex(ad => ad.UserName);
            builder.Property(u => u.UserName).HasMaxLength(50);
        }
    }
}

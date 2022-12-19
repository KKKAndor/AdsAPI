using Ads.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ads.Persistence.EntityTypeConfigurations
{
    public class UserConfiguration : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.HasKey(ad => ad.Id);
            builder.HasIndex(ad => ad.Id).IsUnique();
            builder.Property(u => u.Name).HasMaxLength(50);
        }
    }
}

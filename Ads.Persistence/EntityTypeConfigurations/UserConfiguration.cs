using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Ads.Domain.Entities;

namespace Ads.Persistence.EntityTypeConfigurations
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

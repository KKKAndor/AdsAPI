using Ads.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ads.Persistence.EntityTypeConfigurations
{
    public class AdConfiguration : IEntityTypeConfiguration<Ad>
    {
        public void Configure(EntityTypeBuilder<Ad> builder)
        {
            builder
                .HasKey(ad => ad.Id);
            builder.HasQueryFilter(x=>x.ExpirationDate.Date > DateTime.Now.Date);
            builder
                .HasIndex(ad => ad.Id)
                .IsUnique();
            builder
                .HasIndex(ad => ad.CreationDate);
            builder
                .HasIndex(ad => ad.Number);
            builder
                .HasIndex(ad => ad.Rating);
            builder
                .Property(ad => ad.UserId)
                .IsRequired();
            builder
                .HasOne(ad => ad.User)
                .WithMany(u => u.Ads)
                .HasForeignKey(ad => ad.UserId);
        }
    }
}

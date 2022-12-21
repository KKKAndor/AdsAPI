using Ads.Domain;
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
            builder
                .HasIndex(ad => new { ad.Id, ad.UserId, ad.Number})
                .IsUnique();
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

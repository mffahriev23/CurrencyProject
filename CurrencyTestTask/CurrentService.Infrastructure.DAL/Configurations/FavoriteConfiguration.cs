using CurrencyService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CurrentService.Infrastructure.DAL.Configurations
{
    public class FavoriteConfiguration : IEntityTypeConfiguration<Favorite>
    {
        public void Configure(EntityTypeBuilder<Favorite> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
              .HasField("_id")
              .UsePropertyAccessMode(PropertyAccessMode.Field);

            builder.Property(x => x.UserId)
              .HasField("_userId")
              .UsePropertyAccessMode(PropertyAccessMode.Field);

            builder.Property(x => x.CurrencyId)
              .HasField("_currencyId")
              .UsePropertyAccessMode(PropertyAccessMode.Field);

            builder.Property(x => x.IsActive)
              .HasField("_isActive")
              .UsePropertyAccessMode(PropertyAccessMode.Field);

            builder.HasOne(x => x.Currency)
                .WithMany()
                .HasForeignKey(x => x.CurrencyId);
        }
    }
}

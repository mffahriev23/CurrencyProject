using CurrencyService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CurrentService.Infrastructure.DAL.Configurations
{
    public class FavoriteHistoryConfiguration : IEntityTypeConfiguration<FavoriteHistory>
    {
        public void Configure(EntityTypeBuilder<FavoriteHistory> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
              .HasField("_id")
              .UsePropertyAccessMode(PropertyAccessMode.Field);

            builder.Property(x => x.FavoriteId)
             .HasField("_favoriteId")
             .UsePropertyAccessMode(PropertyAccessMode.Field);

            builder.Property(x => x.Action)
             .HasField("_action")
             .UsePropertyAccessMode(PropertyAccessMode.Field)
             .HasConversion<string>();

            builder.Property(x => x.Created)
             .HasField("_created")
             .UsePropertyAccessMode(PropertyAccessMode.Field);

            builder.HasOne(x => x.Favorite)
                .WithMany(x => x.FavoriteEvents)
                .HasForeignKey(x => x.FavoriteId);
        }
    }
}

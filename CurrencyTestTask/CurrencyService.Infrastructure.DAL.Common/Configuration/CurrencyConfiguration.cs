using CurrencyService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CurrencyService.Infrastructure.DAL.Common.Configurations
{
    public class CurrencyConfiguration : IEntityTypeConfiguration<Currency>
    {
        public void Configure(EntityTypeBuilder<Currency> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
              .HasField("_id")
              .UsePropertyAccessMode(PropertyAccessMode.Field);

            builder.Property(x => x.Name)
              .HasField("_name")
              .UsePropertyAccessMode(PropertyAccessMode.Field);

            builder.Property(x => x.Rate)
              .HasField("_rate")
              .UsePropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}

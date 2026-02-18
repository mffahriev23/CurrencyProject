using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserService.Domain.Entities;

namespace UserService.Infrastructure.DAL.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .IsRequired();

            builder.Property(x => x.Name)
                .IsRequired();

            builder.Property(x => x.Password)
                .IsRequired();

            builder.HasAlternateKey(x => x.Name);

            builder.HasIndex(x => x.Name)
                .IsUnique();
        }
    }
}

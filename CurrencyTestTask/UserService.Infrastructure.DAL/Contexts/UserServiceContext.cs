using Microsoft.EntityFrameworkCore;
using UserService.Domain.Entities;
using UserService.Infrastructure.DAL.Configurations;

namespace UserService.Infrastructure.DAL.Contexts
{
    public class UserServiceContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public UserServiceContext(
            DbContextOptions<UserServiceContext> options
        ) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("user");

            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new RefreshTokenConfiguration());
        }
    }
}

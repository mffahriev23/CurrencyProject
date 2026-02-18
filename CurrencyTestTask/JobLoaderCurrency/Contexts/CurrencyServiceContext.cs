using CurrencyService.Domain.Entities;
using JobLoaderCurrency.Infrastructure.DAL.Configurations;
using Microsoft.EntityFrameworkCore;

namespace CurrentService.Infrastructure.DAL.Contexts
{
    public class CurrencyServiceContext : DbContext
    {

        public DbSet<Currency> Currencies { get; set; }

        public CurrencyServiceContext(
            DbContextOptions<CurrencyServiceContext> options
        ): base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("application");

            modelBuilder.ApplyConfiguration(new CurrencyConfiguration());
        }
    }
}

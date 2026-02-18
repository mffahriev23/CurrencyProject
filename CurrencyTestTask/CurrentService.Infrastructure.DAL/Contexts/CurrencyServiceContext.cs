using CurrencyService.Domain.Entities;
using CurrentService.Infrastructure.DAL.Configurations;
using CurrentService.Infrastructure.DAL.Interceptors;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CurrentService.Infrastructure.DAL.Contexts
{
    public class CurrencyServiceContext : DbContext
    {
        private readonly IMediator? _mediator;

        public DbSet<Currency> Currencies { get; set; }

        public DbSet<Favorite> Favorites { get; set; }

        public DbSet<FavoriteHistory> Histories { get; set; }

        public CurrencyServiceContext(
            DbContextOptions<CurrencyServiceContext> options,
            IMediator mediator
        )
            : base(options)
        {
            _mediator = mediator;
        }

        public CurrencyServiceContext(
            DbContextOptions<CurrencyServiceContext> options
        )
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("application");

            modelBuilder.ApplyConfiguration(new CurrencyConfiguration());
            modelBuilder.ApplyConfiguration(new FavoriteConfiguration());
            modelBuilder.ApplyConfiguration(new FavoriteHistoryConfiguration());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.AddInterceptors(new DomainEventInterceptor(_mediator!));

            base.OnConfiguring(optionsBuilder);
        }
    }
}

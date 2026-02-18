using CurrencyService.Application.Repositories;
using CurrencyService.Application.UnitOfWork;
using CurrentService.Infrastructure.DAL.Contexts;
using CurrentService.Infrastructure.DAL.Repositories;
using JobLoaderCurrency.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CurrentService.Infrastructure.DAL
{
    public static class DependencyInjections
    {
        public static IServiceCollection AddDbContext(
            this IServiceCollection services,
            string? connectionString
        )
        {
            services.AddScoped<IUnitOfWork, UnitOfWork<CurrencyServiceContext>>();
            services.AddDbContext<CurrencyServiceContext>(
                options => options.UseNpgsql(connectionString)
            );

            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<ICurrencyRepository, CurrencyRepository>();
            services.AddScoped<IFavoriteRepository, FavoriteRepository>();
            services.AddScoped<IHistoryRepository, HistoryRepository>();

            return services;
        }

        public static IServiceCollection AddDALServices(
            this IServiceCollection services,
            string? connectionString
        )
        {
            return services.AddDbContext(connectionString)
                .AddRepositories();
        }
    }
}

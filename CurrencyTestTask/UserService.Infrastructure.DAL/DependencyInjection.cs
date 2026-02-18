
using JobLoaderCurrency.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using UserService.Application.Repositories;
using UserService.Application.UnitOfWork;
using UserService.Infrastructure.DAL.Contexts;
using UserService.Infrastructure.DAL.Repositories;

namespace UserService.Infrastructure.DAL
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDbContext(
            this IServiceCollection services,
            string? connectionString
        )
        {
            services.AddScoped<IUnitOfWork, UnitOfWork<UserServiceContext>>();
            services.AddDbContext<UserServiceContext>(
                options => options.UseNpgsql(connectionString)
            );

            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

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

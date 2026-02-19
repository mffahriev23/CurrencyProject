using Application.UnitOfWork;
using Infrastructure.DAL.Ef.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.DAL.Ef
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddUnitOfWork<TDbContext>(this IServiceCollection services)
            where TDbContext : DbContext
        {
            services.AddScoped<IUnitOfWork, UnitOfWork<TDbContext>>();

            return services;
        }
    }
}

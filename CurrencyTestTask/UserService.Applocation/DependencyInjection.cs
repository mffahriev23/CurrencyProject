using Microsoft.Extensions.DependencyInjection;
using UserService.Application.Interfaces;
using UserService.Application.Services;

namespace UserService.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplocatiinServices(this IServiceCollection services)
        {
            services.AddMediatR(options => options.RegisterServicesFromAssemblyContaining(typeof(DependencyInjection)));
            services.AddInternalServices();

            return services;
        }

        public static IServiceCollection AddInternalServices(this IServiceCollection services)
        {
            services.AddTransient<IHasher, HasherService>();

            return services;
        }
    }
}

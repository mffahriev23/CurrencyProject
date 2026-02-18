using Microsoft.Extensions.DependencyInjection;

namespace CurrencyService.Application
{
    public static class DependecyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            return services.AddMediatR(options => options.RegisterServicesFromAssemblyContaining(typeof(DependecyInjection)));
        }
    }
}

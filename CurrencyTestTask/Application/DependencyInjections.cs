using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Exceptions;

namespace Application
{
    public static class DependencyInjections
    {
        public static IServiceCollection AddSerilog(
            this IServiceCollection services,
            IConfiguration configuration,
            IHostBuilder hostBuilder
        )
        {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .Enrich.WithExceptionDetails()
                .WriteTo.Console()
                .CreateLogger();

            hostBuilder.UseSerilog();

            return services;
        }
    }
}

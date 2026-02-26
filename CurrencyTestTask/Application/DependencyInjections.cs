using Application.Interfaces;
using Application.Options;
using Application.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Exceptions;

namespace Application
{
    public static class DependencyInjections
    {
        public static IServiceCollection AddAuthorizationServices(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            services.Configure<JwtManagerOptions>(configuration.GetSection(nameof(JwtManagerOptions)));

            services.AddTransient<IJwtFactory, JwtFactory>();
            services.AddTransient<IJwtReader, JwtReader>();

            return services;
        }

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

using Authorization.Handlers;
using Authorization.Interfaces;
using Authorization.Middlewares;
using Authorization.Options;
using Authorization.Services;
using Microsoft.AspNetCore.Authentication;
using Serilog;
using Serilog.Exceptions;

namespace Authorization
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddAuthorizationHandler(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            services.Configure<JwtManagerOptions>(configuration.GetSection(nameof(JwtManagerOptions)));

            services.AddTransient<IJwtFactory, JwtFactory>();
            services.AddTransient<IJwtReader, JwtReader>();

            services.AddAuthentication("CustomScheme")
                .AddScheme<AuthenticationSchemeOptions, AppAuthenticationHandler>("CustomScheme", null);

            return services;
        }

        public static IServiceCollection AddGlobalExceptionGandler(
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

            services.AddExceptionHandler<GlobalExceptionHandler>();
            services.AddProblemDetails();

            return services;
        }
    }
}

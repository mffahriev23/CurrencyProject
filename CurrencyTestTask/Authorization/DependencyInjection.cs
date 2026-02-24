using Authorization.Handlers;
using Authorization.Interfaces;
using Authorization.Options;
using Authorization.Services;
using Microsoft.AspNetCore.Authentication;

namespace Authorization
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddHandler(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JwtManagerOptions>(configuration.GetSection(nameof(JwtManagerOptions)));

            services.AddTransient<IJwtFactory, JwtFactory>();
            services.AddTransient<IJwtReader, JwtReader>();

            services.AddAuthentication("CustomScheme")
                .AddScheme<AuthenticationSchemeOptions, AppAuthenticationHandler>("CustomScheme", null);

            return services;
        }
    }
}

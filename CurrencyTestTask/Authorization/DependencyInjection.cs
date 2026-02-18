using Authorization.Handlers;
using Authorization.Interfaces;
using Authorization.Services;
using Microsoft.AspNetCore.Authentication;

namespace Authorization
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddHandler(this IServiceCollection services)
        {
            services.AddTransient<IJwtManager, JwtManager>();

            services.AddAuthentication("CustomScheme")
                .AddScheme<AuthenticationSchemeOptions, AppAuthenticationHandler>("CustomScheme", null);

            return services;
        }
    }
}

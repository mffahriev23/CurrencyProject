using WebHost.Handlers;
using Application.Interfaces;
using WebHost.Middlewares;
using Application.Options;
using Application.Services;
using Microsoft.AspNetCore.Authentication;
using Serilog;
using Serilog.Exceptions;

namespace WebHost
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddAuthorizationHandler(
            this IServiceCollection services
        )
        {
            services.AddAuthentication("CustomScheme")
                .AddScheme<AuthenticationSchemeOptions, AppAuthenticationHandler>("CustomScheme", null);

            return services;
        }

        public static IServiceCollection AddGlobalExceptionGandler(
            this IServiceCollection services
        )
        {
            services.AddExceptionHandler<GlobalExceptionHandler>();
            services.AddProblemDetails();

            return services;
        }
    }
}

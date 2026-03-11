using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using WebHost.Middlewares;

namespace WebHost
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddGlobalExceptionHandler(
            this IServiceCollection services
        )
        {
            services.AddExceptionHandler<GlobalExceptionHandler>();
            services.AddProblemDetails();

            return services;
        }

        public static IApplicationBuilder UseAuthorizationAndAuthentication(
            this IApplicationBuilder builder
        )
        {
            builder.UseAuthentication();
            builder.UseAuthorization();

            return builder;
        }

        public static IServiceCollection AddAuthorization(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
            {
                string keyConfig = configuration["Jwt:Key"]!;
                string issuer = configuration["Jwt:Issuer"]!;
                string audience = configuration["Jwt:Audience"]!;

                byte[] key = Encoding.UTF8.GetBytes(keyConfig);

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = issuer,
                    ValidAudience = audience,

                    IssuerSigningKey = new SymmetricSecurityKey(key),
                };
            });

            services.AddAuthorization();

            return services;
        }
    }
}

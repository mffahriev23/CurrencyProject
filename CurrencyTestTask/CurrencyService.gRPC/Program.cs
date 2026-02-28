using System.Text;
using CurrencyService.Application;
using CurrencyService.gRPC.Interceptors;
using CurrencyService.gRPC.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using CurrentService.Infrastructure.DAL;
using Application;

class Program
{
    static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        IConfiguration configuration = builder.Configuration;

        string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

        builder.Services.AddApplicationServices()
            .AddDALServices(connectionString);

        builder.Services.AddSerilog(
            configuration,
            builder.Host
        );

        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                byte[] key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!);

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ClockSkew = TimeSpan.Zero
                };
            });

        builder.Services.AddAuthorization();

        builder.Services.AddScoped<ExceptionInterceptor>();

        builder.Services.AddGrpc(options =>
            options.Interceptors.Add<ExceptionInterceptor>()
        );

        WebApplication app = builder.Build();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapGrpcService<GreeterService>();
        app.MapGrpcService<CurrenncyServer>();

        app.Run();
    }
}
using CurrencyService.Application;
using CurrentService.Infrastructure.DAL;
using Application;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using WebHost;

class Program
{
    static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        IConfiguration configuration = builder.Configuration;

        string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

        builder.Services.AddGlobalExceptionHandler();

        builder.Services.AddSerilog(
            configuration,
            builder.Host
        );

        builder.Services.AddAuthorization(configuration);

        builder.Services.AddApplicationServices()
            .AddDALServices(connectionString);

        WebApplication app = builder.Build();

        app.UseExceptionHandler();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorizationAndAuthentication();

        app.MapControllers();

        app.Run();
    }
}
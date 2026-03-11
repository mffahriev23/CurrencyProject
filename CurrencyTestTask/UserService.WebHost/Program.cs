using WebHost;
using Serilog;
using UserService.Application;
using UserService.Infrastructure.DAL;
using Application;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.IdentityModel.Tokens;

class Program
{
    static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        IConfiguration configuration = builder.Configuration;

        string? connectionString = configuration.GetConnectionString("DefaultConnection");

        builder.Services.AddSerilog(
            configuration,
            builder.Host
        );

        builder.Services.AddGlobalExceptionHandler();

        builder.Services.AddAuthorization(configuration);

        builder.Services.AddApplicatiinServices()
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
using Authorization;
using Serilog;
using UserService.Application;
using UserService.Infrastructure.DAL;
using Application;

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

        builder.Services.AddApplicatiinServices()
            .AddDALServices(connectionString)
            .AddAuthorizationServices(configuration)
            .AddAuthorizationHandler();

        builder.Services.AddSerilog(
            configuration,
            builder.Host
        ).AddGlobalExceptionGandler();

        var app = builder.Build();

        app.UseExceptionHandler();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
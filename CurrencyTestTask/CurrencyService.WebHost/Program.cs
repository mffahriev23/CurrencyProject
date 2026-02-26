using CurrencyService.Application;
using CurrentService.Infrastructure.DAL;
using Authorization;
using Application;

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

        builder.Services.AddApplicationServices()
            .AddDALServices(connectionString)
            .AddAuthorizationServices(configuration)
            .AddAuthorizationHandler();

        builder.Services.AddSerilog(
            configuration,
            builder.Host
        ).AddGlobalExceptionGandler();

        WebApplication app = builder.Build();

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
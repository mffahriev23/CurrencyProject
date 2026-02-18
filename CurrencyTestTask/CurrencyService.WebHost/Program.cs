using CurrencyService.Application;
using CurrentService.Infrastructure.DAL;
using Authorization;

class Program
{
    static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

        builder.Services.AddApplicationServices()
            .AddDALServices(connectionString)
            .AddHandler();

        WebApplication app = builder.Build();

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
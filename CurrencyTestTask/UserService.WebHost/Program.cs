using Authorization;
using UserService.Application;
using UserService.Infrastructure.DAL;

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
            .AddAuthorizationHandler(configuration);

        builder.Services.AddGlobalExceptionGandler(
            configuration,
            builder.Host
        );

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
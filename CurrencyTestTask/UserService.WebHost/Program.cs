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

        string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

        builder.Services.AddApplocatiinServices()
            .AddDALServices(connectionString)
            .AddHandler();

        var app = builder.Build();

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
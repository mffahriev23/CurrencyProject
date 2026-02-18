using CurrentService.Infrastructure.DAL.Contexts;
using Microsoft.EntityFrameworkCore;
using UserService.Infrastructure.DAL.Contexts;

class Program
{
    static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        string? currencyServiceConnection = builder.Configuration.GetConnectionString("CurrencyServiceConnection");
        string? userServiceConnection = builder.Configuration.GetConnectionString("UserServiceConnection");

        builder.Services.AddDbContext<CurrencyServiceContext>(
            options => options.UseNpgsql(currencyServiceConnection, b => b.MigrationsAssembly("MigrationBundel"))
        );

        builder.Services.AddDbContext<UserServiceContext>(
            options => options.UseNpgsql(userServiceConnection, b => b.MigrationsAssembly("MigrationBundel"))
        );

        WebApplication app = builder.Build();

        using IServiceScope scope = app.Services.CreateScope();

        IServiceProvider services = scope.ServiceProvider;

        CurrencyServiceContext currencyContext = services.GetRequiredService<CurrencyServiceContext>();

        currencyContext.Database.Migrate();

        UserServiceContext userContext = services.GetRequiredService<UserServiceContext>();

        userContext.Database.Migrate();
    }
}
using CurrentService.Infrastructure.DAL.Contexts;
using JobLoaderCurrency.Clients;
using JobLoaderCurrency.Interfaces.CurrencyLoader;
using JobLoaderCurrency.Interfaces.Repository;
using JobLoaderCurrency.Interfaces.Services;
using JobLoaderCurrency.Jobs;
using JobLoaderCurrency.Repositories;
using JobLoaderCurrency.Services;
using Microsoft.EntityFrameworkCore;
using Infrastructure.DAL.Ef;

class Program
{
    static void Main(string[] args)
    {
        System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

        builder.Services.AddScoped<ICurrencyRepository, CurrencyRepository>();
        builder.Services.AddScoped<IUpdateCurrency, UpdateCurrency>();
        builder.Services.AddUnitOfWork<CurrencyServiceContext>();

        builder.Services.AddDbContext<CurrencyServiceContext>(
            options => options.UseNpgsql(connectionString)
        );

        builder.Services.AddHttpClient<ICurrencyLoaderClient, CurrencyLoaderClient>(
            client => client.BaseAddress = new Uri("https://www.cbr.ru")
        );

        builder.Services.AddHostedService<LoaderCurrencies>();

        WebApplication app = builder.Build();

        app.Run();
    }
}
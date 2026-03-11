using CurrencyService.Application;
using CurrencyService.gRPC.Interceptors;
using CurrencyService.gRPC.Services;
using CurrentService.Infrastructure.DAL;
using Application;
using WebHost;

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

        builder.Services.AddAuthorization(configuration);

        builder.Services.AddScoped<ExceptionInterceptor>();

        builder.Services.AddGrpc(options =>
            options.Interceptors.Add<ExceptionInterceptor>()
        );

        WebApplication app = builder.Build();

        app.UseAuthorizationAndAuthentication();

        app.MapGrpcService<GreeterService>();
        app.MapGrpcService<CurrenncyServer>();

        app.Run();
    }
}
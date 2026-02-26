using ApiGateway;
using ApiGateway.Clients;
using ApiGateway.Interfaces.UserService;
using Authorization;
using Application;

class Program
{
    static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        IConfiguration configuration = builder.Configuration;

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services
            .AddAuthorizationServices(configuration)
            .AddAuthorizationHandler();

        builder.Services.RegistrationHttpClient<IUserServiceClient, UserServiceClient>(
            "UserService",
            configuration
        );

        builder.Services.RegistrationHttpClient<ICurrencyServiceClient, CurrencyServiceClient>(
            "CurrencyService",
            configuration
        );

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
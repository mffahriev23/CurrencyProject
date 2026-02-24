using ApiGateway;
using ApiGateway.Clients;
using ApiGateway.Interfaces.UserService;
using Authorization;

class Program
{
    static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        IConfiguration configuration = builder.Configuration;

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddAuthorizationHandler(configuration);

        builder.Services.RegistrationHttpClient<IUserServiceClient, UserServiceClient>(
            "UserService",
            configuration
        );

        builder.Services.RegistrationHttpClient<ICurrencyServiceClient, CurrencyServiceClient>(
            "CurrencyService",
            configuration
        );

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
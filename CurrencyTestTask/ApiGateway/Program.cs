using ApiGateway;
using ApiGateway.Clients;
using ApiGateway.Interfaces.UserService;
using WebHost;

class Program
{
    static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        IConfiguration configuration = builder.Configuration;

        builder.Services.AddGlobalExceptionHandler();

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.RegistrationHttpClient<IUserServiceClient, UserServiceClient>(
            "UserService",
            configuration
        );

        builder.Services.RegistrationHttpClient<ICurrencyServiceClient, CurrencyServiceClient>(
            "CurrencyService",
            configuration
        );

        builder.Services.AddAuthorization(configuration);

        var app = builder.Build();

        app.UseExceptionHandler();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorizationAndAuthentication();

        app.MapControllers();

        app.Run();
    }
}
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

        builder.Services.AddHandler(configuration);

        builder.Services.RegistrationHttpClient<IUserServiceClient, UserServiceClient>(
            "UserService",
            configuration
        );

        builder.Services.RegistrationHttpClient<ICurrencyServiceClient, CurrencyServiceClient>(
            "CurrencyService",
            configuration
        );

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
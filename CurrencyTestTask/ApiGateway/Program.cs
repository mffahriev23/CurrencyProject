using ApiGateway.Clients;
using ApiGateway.Interfaces.UserService;
using Authorization;

class Program
{
    static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddHandler();

        builder.Services.AddHttpClient<IUserServiceClient, UserServiceClient>(
            client => client.BaseAddress = new Uri("https://localhost:7028")
        );

        builder.Services.AddHttpClient<ICurrencyServiceClient, CurrencyServiceClient> (
            client => client.BaseAddress = new Uri("https://localhost:7049")
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
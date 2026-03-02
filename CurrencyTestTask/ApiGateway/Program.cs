using ApiGateway;
using ApiGateway.Clients;
using ApiGateway.Interfaces.UserService;
using WebHost;
using Application;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.IdentityModel.Tokens;

class Program
{
    static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        IConfiguration configuration = builder.Configuration;

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddAuthorizationServices(configuration)
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
        );

        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                byte[] key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!);

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],

                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ClockSkew = TimeSpan.Zero
                };
            });

        builder.Services.AddAuthorization();

        var app = builder.Build();

        app.UseExceptionHandler();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();
        app.UseAuthentication();

        app.MapControllers();

        app.Run();
    }
}
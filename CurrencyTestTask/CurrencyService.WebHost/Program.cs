using CurrencyService.Application;
using CurrentService.Infrastructure.DAL;
using WebHost;
using Application;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.IdentityModel.Tokens;

class Program
{
    static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        IConfiguration configuration = builder.Configuration;

        string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

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

        builder.Services.AddApplicationServices()
            .AddDALServices(connectionString);

        WebApplication app = builder.Build();

        app.UseExceptionHandler();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
using ApiGateway.Options;
using Microsoft.Extensions.Options;

namespace ApiGateway
{
    public static class DependencyInjection
    {
        public static IServiceCollection RegistrationHttpClient<TClient, TImplementation>(
            this IServiceCollection services,
            string configName,
            IConfiguration configuration
        )
            where TClient : class
            where TImplementation : class, TClient
        {
            services.Configure<ExternalHttpClientOptions>(
                configName,
                configuration.GetSection(configName)
            );

            services.AddHttpClient<TClient, TImplementation>(
                (sp, client) =>
                {
                    ExternalHttpClientOptions option =
                sp.GetRequiredService<IOptionsMonitor<ExternalHttpClientOptions>>()
                    .Get(configName);

                    client.BaseAddress = new Uri(option.Url!);
                }
            );

            return services;
        }
    }
}

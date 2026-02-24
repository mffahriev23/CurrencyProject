using JobLoaderCurrency.Interfaces.CurrencyLoader;
using JobLoaderCurrency.Interfaces.CurrencyLoader.Dtos;
using JobLoaderCurrency.Interfaces.Services;

namespace JobLoaderCurrency.Jobs
{
    public class LoaderCurrencies : BackgroundService
    {
        readonly IServiceScopeFactory _serviceScopeFactory;
        readonly ILogger<LoaderCurrencies> _logger;

        public LoaderCurrencies(IServiceScopeFactory serviceScopeFactory, ILogger<LoaderCurrencies> logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationTocken)
        {
            while (true)
            {
                _logger.LogInformation("Обновление курсов валют...");

                try
                {
                    using (IServiceScope scope = _serviceScopeFactory.CreateScope())
                    {
                        ICurrencyLoaderClient client = scope.ServiceProvider.GetRequiredService<ICurrencyLoaderClient>();
                        IUpdateCurrency updater = scope.ServiceProvider.GetRequiredService<IUpdateCurrency>();

                        ValCursDto data = await client.GetCurrencies(cancellationTocken);
                        ValuteDto[] valutes = data?.Valutes ?? Array.Empty<ValuteDto>();

                        await updater.Execute(data.Valutes, cancellationTocken);
                    }

                    _logger.LogInformation("Обновление курсов валют завершено.");

                    await Task.Delay(TimeSpan.FromDays(1), cancellationTocken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Ошибка в работе джобы.");
                }
            }
        }
    }
}

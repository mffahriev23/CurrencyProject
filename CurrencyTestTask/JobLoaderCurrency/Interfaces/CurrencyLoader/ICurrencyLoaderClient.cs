using JobLoaderCurrency.Interfaces.CurrencyLoader.Dtos;

namespace JobLoaderCurrency.Interfaces.CurrencyLoader
{
    public interface ICurrencyLoaderClient
    {
        Task<ValCursDto> GetCurrencies(CancellationToken cancellationToken);
    }
}

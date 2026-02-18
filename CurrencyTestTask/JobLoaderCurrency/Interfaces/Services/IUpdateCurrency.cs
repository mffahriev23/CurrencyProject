using JobLoaderCurrency.Interfaces.CurrencyLoader.Dtos;

namespace JobLoaderCurrency.Interfaces.Services
{
    public interface IUpdateCurrency
    {
        Task Execute(ValuteDto[] actualData, CancellationToken cancellationToken);
    }
}

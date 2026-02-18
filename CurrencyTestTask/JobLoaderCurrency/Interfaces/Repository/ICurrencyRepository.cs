using CurrencyService.Domain.Entities;

namespace JobLoaderCurrency.Interfaces.Repository
{
    public interface ICurrencyRepository
    {
        Task<Currency[]> GetAll(CancellationToken cancellationToken);

        void AddRange(Currency[] currencies);
    }
}

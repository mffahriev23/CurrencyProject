using CurrencyService.Domain.Entities;
using CurrentService.Infrastructure.DAL.Contexts;
using JobLoaderCurrency.Interfaces.Repository;
using Microsoft.EntityFrameworkCore;

namespace JobLoaderCurrency.Repositories
{
    public class CurrencyRepository : ICurrencyRepository
    {
        readonly CurrencyServiceContext _currencyServiceContext;

        public CurrencyRepository(CurrencyServiceContext currencyServiceContext)
        {
            _currencyServiceContext = currencyServiceContext;
        }

        public void AddRange(Currency[] currencies)
        {
            _currencyServiceContext.Currencies.AddRange(currencies);
        }

        public Task<Currency[]> GetAll(CancellationToken cancellationToken)
        {
            return _currencyServiceContext.Currencies.ToArrayAsync(cancellationToken);
        }
    }
}

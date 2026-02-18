using CurrencyService.Application.Repositories;
using CurrencyService.Domain.Entities;
using CurrentService.Infrastructure.DAL.Contexts;

namespace CurrentService.Infrastructure.DAL.Repositories
{
    class HistoryRepository : IHistoryRepository
    {
        readonly CurrencyServiceContext _currencyServiceContext;

        public HistoryRepository(CurrencyServiceContext currencyServiceContext)
        {
            _currencyServiceContext = currencyServiceContext;
        }

        public void Add(FavoriteHistory history)
        {
            _currencyServiceContext.Histories.Add(history);
        }
    }
}

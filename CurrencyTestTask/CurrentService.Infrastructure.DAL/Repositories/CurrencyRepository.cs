using CurrencyService.Application.Repositories;
using CurrentService.Infrastructure.DAL.Contexts;
using Microsoft.EntityFrameworkCore;

namespace CurrentService.Infrastructure.DAL.Repositories
{
    class CurrencyRepository : ICurrencyRepository
    {
        readonly CurrencyServiceContext _currencyServiceContext;

        public CurrencyRepository(CurrencyServiceContext currencyServiceContext)
        {
            _currencyServiceContext = currencyServiceContext;
        }

        public async Task<(Guid id, string name)[]> GetAllNames(CancellationToken cancellationToken)
        {
            return (await _currencyServiceContext.Currencies
                    .Select(x => new { x.Id, x.Name })
                    .ToArrayAsync(cancellationToken)
                ).Select(x => (x.Id, x.Name))
                .ToArray();
        }
    }
}

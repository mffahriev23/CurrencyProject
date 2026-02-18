using CurrencyService.Application.Repositories;
using CurrencyService.Domain.Entities;
using CurrentService.Infrastructure.DAL.Contexts;
using Microsoft.EntityFrameworkCore;

namespace CurrentService.Infrastructure.DAL.Repositories
{
    public class FavoriteRepository : IFavoriteRepository
    {
        readonly CurrencyServiceContext _currencyServiceContext;

        public FavoriteRepository(CurrencyServiceContext currencyServiceContext)
        {
            _currencyServiceContext = currencyServiceContext;
        }

        public void AddRange(Favorite[] favorites)
        {
            _currencyServiceContext.Favorites.AddRange(favorites);
        }

        public Task<Favorite[]> GetByUserIdNoTracking(
            Guid userId,
            CancellationToken cancellationToken
        )
        {
            return _currencyServiceContext.Favorites
                .AsNoTracking()
                .Include(x => x.Currency)
                .Where(x => x.UserId == userId)
                .Where(x => x.IsActive)
                .ToArrayAsync(cancellationToken);
        }

        public Task<Favorite[]> GetByUserId(
            Guid userId,
            CancellationToken cancellationToken
        )
        {
            return _currencyServiceContext.Favorites
                .Include(x => x.Currency)
                .Where(x => x.UserId == userId)
                .ToArrayAsync(cancellationToken);
        }
    }
}

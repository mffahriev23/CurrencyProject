using CurrencyService.Domain.Entities;

namespace CurrencyService.Application.Repositories
{
    public interface IFavoriteRepository
    {
        Task<Favorite[]> GetByUserIdNoTracking(Guid userId, CancellationToken cancellationToken);

        Task<Favorite[]> GetByUserId(
            Guid userId,
            CancellationToken cancellationToken
        );

        void AddRange(Favorite[] favorites);
    }
}


using CurrencyService.Application.Repositories;
using CurrencyService.Domain.Entities;
using MediatR;

namespace CurrencyService.Application.Favorites.Queries.GetFavorites
{
    public class GetFavoritesQueryHandler : IRequestHandler<GetFavoritesQuery, FavoriteItem[]>
    {
        readonly IFavoriteRepository _favoriteRepository;

        public GetFavoritesQueryHandler(
            IFavoriteRepository favoriteRepository
        )
        {
            _favoriteRepository = favoriteRepository;
        }

        public async Task<FavoriteItem[]> Handle(GetFavoritesQuery request, CancellationToken cancellationToken)
        {
             Favorite[] favorites = await _favoriteRepository.GetByUserIdNoTracking(
                 request.UserId,
                 cancellationToken
             );

            return favorites.Select(x => new FavoriteItem(
                    x.Id,
                    x.Currency!.Id,
                    x.Currency.Name,
                    x.Currency.Rate
                )
            ).ToArray();
        }
    }
}

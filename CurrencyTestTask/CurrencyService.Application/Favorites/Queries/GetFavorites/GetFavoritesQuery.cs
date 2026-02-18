using MediatR;

namespace CurrencyService.Application.Favorites.Queries.GetFavorites
{
    public record GetFavoritesQuery(Guid UserId) : IRequest<FavoriteItem[]>;
}

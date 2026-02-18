using CurrencyService.Application.Favorites.Commands.AddFavorite;
using CurrencyService.Application.Favorites.Commands.RemoveFavorite;
using CurrencyService.Application.Favorites.Queries.GetFavorites;
using CurrencyService.Contracts.Favorite.AddFavorite;
using CurrencyService.Contracts.Favorite.DeleteFavorite;
using CurrencyService.Contracts.Favorite.GetFavorites;

namespace CurrencyService.WebHost.Mappings
{
    public static class FavoriteMappings
    {
        public static AddFavoriteCommand Map(this AddFavoriteRequest request, Guid userId)
        {
            return new(request.Body!.CurrencyIds, userId);
        }

        public static RemoveFavoriteCommand Map(this DeleteFavoriteRequest request, Guid userId)
        {
            return new(request.Body!.CurrencyIds, userId);
        }

        public static FavoritesResponse Map(this FavoriteItem[] items)
        {
            return new()
            {
                Items = items.Select(x => new FavoriteDataItem
                {
                    Id = x.Id,
                    CurrencyId = x.CurrencyId,
                    Name = x.Name,
                    Rate = x.Rate,
                }).ToArray()
            };
        }
    }
}

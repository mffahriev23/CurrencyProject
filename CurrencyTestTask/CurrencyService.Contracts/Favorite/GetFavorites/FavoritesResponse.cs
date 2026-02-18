using System.Text.Json.Serialization;

namespace CurrencyService.Contracts.Favorite.GetFavorites
{
    public record FavoritesResponse
    {
        [JsonPropertyName("items")]
        public FavoriteDataItem[] Items { get; init; }

        public FavoritesResponse()
        {
            Items = Array.Empty<FavoriteDataItem>();
        }
    }
}

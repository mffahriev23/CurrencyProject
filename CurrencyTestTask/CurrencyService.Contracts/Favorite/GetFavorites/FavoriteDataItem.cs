using System.Text.Json.Serialization;

namespace CurrencyService.Contracts.Favorite.GetFavorites
{
    public record FavoriteDataItem
    {
        [JsonPropertyName("id")]
        public Guid Id { get; init; }

        [JsonPropertyName("currencyId")]
        public Guid CurrencyId { get; init; }

        [JsonPropertyName("name")]
        public string? Name { get; init; }

        [JsonPropertyName("rate")]
        public decimal Rate { get; init; }
    }
}

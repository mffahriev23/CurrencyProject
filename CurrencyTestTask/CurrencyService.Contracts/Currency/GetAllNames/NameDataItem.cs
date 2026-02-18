using System.Text.Json.Serialization;

namespace CurrencyService.Contracts.Currency.GetAllNames
{
    public record NameDataItem
    {
        [JsonPropertyName("id")]
        public Guid Id { get; init; }

        [JsonPropertyName("name")]
        public required string Name { get; init; }
    }
}

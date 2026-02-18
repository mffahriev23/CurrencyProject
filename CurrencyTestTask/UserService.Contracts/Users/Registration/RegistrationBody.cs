using System.Text.Json.Serialization;

namespace UserService.Contracts.Users.Registration
{
    public record RegistrationBody
    {
        [JsonPropertyName("name")]
        public string? Name { get; init; }

        [JsonPropertyName("password")]
        public string? Password { get; init; }

        [JsonPropertyName("doublePassword")]
        public string? DoublePassword { get; init; }
    }
}

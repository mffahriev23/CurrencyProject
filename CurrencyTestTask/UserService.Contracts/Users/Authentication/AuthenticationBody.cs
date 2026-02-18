using System.Text.Json.Serialization;

namespace UserService.Contracts.Users.Authentication
{
    public record AuthenticationBody
    {
        [JsonPropertyName("name")]
        public string? Name { get; init; }

        [JsonPropertyName("password")]
        public string? Password { get; init; }
    }
}

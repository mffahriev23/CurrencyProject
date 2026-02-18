using System.Text.Json.Serialization;

namespace UserService.Contracts.Users.Refresh
{
    public record RefreshTokenBody
    {
        [JsonPropertyName("refreshToken")]
        public string? RefreshToken { get; init; }
    }
}

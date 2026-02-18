using System.Text.Json.Serialization;

namespace UserService.Contracts.Users.Refresh
{
    public record RefreshTokenResponse
    {
        [JsonPropertyName("accessToken")]
        public string AccessToken { get; init; }

        [JsonPropertyName("refreshToken")]
        public string RefreshToken { get; init; }
    }
}

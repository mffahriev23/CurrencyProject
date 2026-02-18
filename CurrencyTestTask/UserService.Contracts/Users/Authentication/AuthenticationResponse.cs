using System.Text.Json.Serialization;

namespace UserService.Contracts.Users.Authentication
{
    public record AuthenticationResponse
    {
        [JsonPropertyName("accessToken")]
        public string AccessToken { get; init; }

        [JsonPropertyName("refreshToken")]
        public string RefreshToken { get; init; }
    }
}

using System.Text.Json.Serialization;

namespace UserService.Contracts.Users.Logout
{
    public record LogOutBody
    {
        [JsonPropertyName("refreshToken")]
        public string? RefreshToken { get; init; }
    }
}

using System.Text.Json.Serialization;

namespace Authorization.Dtos
{
    public record AccessTokenClaims
    {
        public string UserId {  get; init; }

        public string Name { get; init; }
    }
}

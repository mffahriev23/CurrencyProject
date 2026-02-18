using System.Text.Json.Serialization;

namespace Authorization.Dtos
{
    public record AuthorizationClaims
    {
        public string UserId {  get; init; }

        public string Name { get; init; }
    }
}

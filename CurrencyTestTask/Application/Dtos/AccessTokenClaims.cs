using System.Text.Json.Serialization;

namespace Application.Dtos
{
    public record AccessTokenClaims
    {
        public string UserId {  get; init; }

        public string Name { get; init; }
    }
}

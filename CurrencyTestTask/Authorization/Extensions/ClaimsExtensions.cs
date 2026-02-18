using System.Security.Claims;
using Authorization.Dtos;

namespace Authorization.Extensions
{
    public static class ClaimsExtensions
    {
        public static Guid GetUserId(this IEnumerable<Claim> claims)
        {
            string userIdText = claims.First(x => x.Type == nameof(AuthorizationClaims.UserId)).Value;

            return new Guid(userIdText);
        }
    }
}

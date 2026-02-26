using System.Security.Claims;
using Application.Dtos;

namespace Application.Extensions
{
    public static class ClaimsExtensions
    {
        public static Guid GetUserId(this IEnumerable<Claim> claims)
        {
            string userIdText = claims.First(x => x.Type == nameof(AccessTokenClaims.UserId)).Value;

            return new Guid(userIdText);
        }

        public static Guid GetRefreshKey(this IEnumerable<Claim> claims)
        {
            string keyText = claims.First(x => x.Type == nameof(RefreshTokenClaims.Key)).Value;

            return new Guid(keyText);
        }
    }
}

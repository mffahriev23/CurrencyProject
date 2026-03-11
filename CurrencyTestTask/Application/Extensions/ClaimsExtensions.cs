using System.Security.Claims;

namespace Application.Extensions
{
    public static class ClaimsExtensions
    {
        public static Guid GetUserId(this IEnumerable<Claim> claims)
        {
            string userIdText = claims.First(x => x.Type.Equals("userId")).Value;

            return new Guid(userIdText);
        }
    }
}

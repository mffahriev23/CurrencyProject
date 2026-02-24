using System.Security.Claims;

namespace Authorization.Interfaces
{
    public interface IJwtReader
    {
        Claim[] GetAccessTokenClaims(string jwtText, bool validateExpirationTime);

        Claim[] GetRefreshTokenClaims(string jwtText);
    }
}

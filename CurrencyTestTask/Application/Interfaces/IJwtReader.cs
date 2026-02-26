using System.Security.Claims;

namespace Application.Interfaces
{
    public interface IJwtReader
    {
        Claim[] GetAccessTokenClaims(string jwtText, bool validateExpirationTime);

        Claim[] GetRefreshTokenClaims(string jwtText);
    }
}

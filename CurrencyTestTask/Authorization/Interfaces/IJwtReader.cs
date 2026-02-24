using System.Security.Claims;

namespace Authorization.Interfaces
{
    public interface IJwtReader
    {
        Claim[] GetClaims(string jwtText, bool validateExpirationTime);
    }
}

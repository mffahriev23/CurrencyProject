using System.Security.Claims;

namespace Authorization.Interfaces
{
    public interface IJwtManager
    {
        string GetJwtToken(Guid userId, string name);

        Claim[] GetClaims(string jwtText, bool validateExpirationTime);
    }
}

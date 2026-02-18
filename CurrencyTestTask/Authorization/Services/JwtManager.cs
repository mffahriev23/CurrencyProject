using System.Security.Claims;
using Authorization.Dtos;
using Authorization.Interfaces;
using JWT;
using JWT.Algorithms;
using JWT.Builder;

namespace Authorization.Services
{
    public class JwtManager : IJwtManager
    {
        const string _jwtSecret ="8FWyWKDPEkZVfq6woDAhr3RLFIDyjqLY";

        public Claim[] GetClaims(string jwtText, bool validateExpirationTime)
        {
            JwtBuilder jwt = JwtBuilder.Create()
                .WithAlgorithm(new HMACSHA256Algorithm())
                .WithSecret(_jwtSecret)
                .WithValidationParameters(
                    new ValidationParameters
                    {
                        ValidateSignature = true,
                        ValidateExpirationTime = validateExpirationTime
                    });

            AuthorizationClaims claims = jwt.Decode<AuthorizationClaims>(jwtText);

            return [
                new(nameof(AuthorizationClaims.Name), claims.Name),
                new(nameof(AuthorizationClaims.UserId), claims.UserId)
            ];
        }

        public string GetJwtToken(Guid userId, string name)
        {
            return JwtBuilder.Create()
                .WithAlgorithm(new HMACSHA256Algorithm())
                .AddClaim(nameof(AuthorizationClaims.UserId), userId.ToString())
                .AddClaim(nameof(AuthorizationClaims.Name), name)
                .WithSecret(_jwtSecret)
                .ExpirationTime(DateTime.UtcNow.AddHours(1))
                .Encode();
        }
    }
}

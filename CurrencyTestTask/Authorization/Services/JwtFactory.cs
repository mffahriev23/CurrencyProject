using System.Security.Claims;
using System.Xml.Linq;
using Authorization.Dtos;
using Authorization.Interfaces;
using Authorization.Options;
using JWT;
using JWT.Algorithms;
using JWT.Builder;
using Microsoft.Extensions.Options;

namespace Authorization.Services
{
    public class JwtFactory : IJwtFactory
    {
        readonly string _jwtSecret;
        readonly int _expirationAccessTokenTime;
        readonly int _expirationRefreshTokenTime;

        public JwtFactory(IOptions<JwtManagerOptions> options)
        {
            _jwtSecret = options.Value.Secret!;
            _expirationAccessTokenTime = options.Value.ExpirationAccessTokenOnMinutes!.Value;
            _expirationRefreshTokenTime = options.Value.ExpirationRefreshTokenOnDay!.Value;
        }

        public string GetJwtToken(Guid userId, string name)
        {
            return JwtBuilder.Create()
                .WithAlgorithm(new HMACSHA256Algorithm())
                .AddClaim(nameof(AccessTokenClaims.UserId), userId.ToString())
                .AddClaim(nameof(AccessTokenClaims.Name), name)
                .WithSecret(_jwtSecret)
                .ExpirationTime(DateTime.UtcNow.AddMinutes(_expirationAccessTokenTime))
                .Encode();
        }

        public string GetJwtToken(Guid key)
        {
            return JwtBuilder.Create()
               .WithAlgorithm(new HMACSHA256Algorithm())
               .AddClaim(nameof(RefreshTokenClaims.Key), key.ToString())
               .WithSecret(_jwtSecret)
               .ExpirationTime(DateTime.UtcNow.AddDays(_expirationRefreshTokenTime))
               .Encode();
        }
    }
}

using System.Security.Claims;
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
        readonly int _expirationTime;

        public JwtFactory(IOptions<JwtManagerOptions> options)
        {
            _jwtSecret = options.Value.Secret!;
            _expirationTime = options.Value.ExpirationTimesOnMinutes!.Value;
        }

        public string GetJwtToken(Guid userId, string name)
        {
            return JwtBuilder.Create()
                .WithAlgorithm(new HMACSHA256Algorithm())
                .AddClaim(nameof(AuthorizationClaims.UserId), userId.ToString())
                .AddClaim(nameof(AuthorizationClaims.Name), name)
                .WithSecret(_jwtSecret)
                .ExpirationTime(DateTime.UtcNow.AddMinutes(_expirationTime))
                .Encode();
        }
    }
}

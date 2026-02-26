using System.Security.Claims;
using Application.Dtos;
using Application.Interfaces;
using Application.Options;
using JWT;
using JWT.Algorithms;
using JWT.Builder;
using Microsoft.Extensions.Options;

namespace Application.Services
{
    public class JwtReader : IJwtReader
    {
        readonly string _jwtSecret;

        public JwtReader(IOptions<JwtManagerOptions> options)
        {
            _jwtSecret = options.Value.Secret!;
        }

        public Claim[] GetAccessTokenClaims(string jwtText, bool validateExpirationTime)
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

            AccessTokenClaims claims = jwt.Decode<AccessTokenClaims>(jwtText);

            return [
                new(nameof(AccessTokenClaims.Name), claims.Name),
                new(nameof(AccessTokenClaims.UserId), claims.UserId)
            ];
        }

        public Claim[] GetRefreshTokenClaims(string jwtText)
        {
            JwtBuilder jwt = JwtBuilder.Create()
                .WithAlgorithm(new HMACSHA256Algorithm())
                .WithSecret(_jwtSecret)
                .WithValidationParameters(
                    new ValidationParameters
                    {
                        ValidateSignature = true,
                        ValidateExpirationTime = true
                    });

            RefreshTokenClaims claims = jwt.Decode<RefreshTokenClaims>(jwtText);

            return [
                new(nameof(RefreshTokenClaims.Key), claims.Key)
            ];
        }
    }
}

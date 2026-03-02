using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Exceptions;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using UserService.Application.Interfaces;

namespace UserService.Application.Users.Commands.RefreshV2
{
    public class RefreshCommandHandler : IRequestHandler<RefreshCommand, RefreshResult>
    {
        readonly IConfiguration _configuration;
        readonly IRefreshTokenService _refreshTokenService;
        readonly ITokenGenerator _tokenGenerator;

        public RefreshCommandHandler(
            IConfiguration configuration,
            IRefreshTokenService refreshTokenService,
            ITokenGenerator tokenGenerator
        )
        {
            _configuration = configuration;
            _refreshTokenService = refreshTokenService;
            _tokenGenerator = tokenGenerator;
        }

        public async Task<RefreshResult> Handle(
            RefreshCommand request,
            CancellationToken cancellationToken
        )
        {
            (Guid userId, string username) = GetUserIdFromExpiredToken(
                request.AccessToken
            );

            bool validRefreshToken = await _refreshTokenService.ValidateRefreshToken(
                userId,
                request.RefreshToken,
                cancellationToken
            );

            if (!validRefreshToken)
            {
                throw new ForbiddenException("Пришёл не валидный токен обновления.");
            }

            await _refreshTokenService.RevokeRefreshToken(
                userId,
                request.RefreshToken,
                cancellationToken
            );

            string accessToken = _tokenGenerator.GenerateAccessToken(userId, username);
            string refreshToken = _tokenGenerator.GenerateRefreshToken();

            await _refreshTokenService.AddRefreshToken(
                userId,
                refreshToken,
                cancellationToken
            );

            return new RefreshResult(accessToken, refreshToken);
        }

        private (Guid userId, string username) GetUserIdFromExpiredToken(string token)
        {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            byte[] key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!);

            try
            {
                ClaimsPrincipal principal = tokenHandler.ValidateToken(
                    token,
                    new TokenValidationParameters
                    {
                        ValidateAudience = true,
                        ValidateIssuer = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = _configuration["Jwt:Issuer"],
                        ValidAudience = _configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateLifetime = false
                    },
                    out SecurityToken validatedToken
                );

                string userIdText = principal.Claims.First(x => x.Type.Equals("userId")).Value;
                string username = principal.Claims.First(x => x.Type.Equals(ClaimTypes.Name)).Value;

                return (new Guid(userIdText), username);
            }
            catch (Exception ex)
            {
                throw new ForbiddenException("Получен невалидный accessToken.", ex);
            }
        }
    }
}

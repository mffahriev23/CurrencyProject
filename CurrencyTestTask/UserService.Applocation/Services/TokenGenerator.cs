using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using UserService.Application.Interfaces;

namespace UserService.Application.Services
{
    public class TokenGenerator : ITokenGenerator
    {
        readonly IConfiguration _configuration;

        public TokenGenerator(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateRefreshToken()
        {
            byte[] bytes = new byte[64];
            using RandomNumberGenerator rng = RandomNumberGenerator.Create();
            rng.GetBytes(bytes);
            return Convert.ToBase64String(bytes);
        }

        public string GenerateAccessToken(Guid userId, string username)
        {
            var claims = new[]
            {
                new Claim("userId", userId.ToString()),
                new Claim(ClaimTypes.Name, username),
            };

            byte[] keyBytes = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!);
            SymmetricSecurityKey key = new SymmetricSecurityKey(keyBytes);
            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(int.Parse(_configuration["Jwt:TokenLifetimeMinutes"]!)),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

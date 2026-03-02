using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using UserService.Application.Interfaces;

namespace UserService.Application.Services
{
    public class RefreshTokenService : IRefreshTokenService
    {
        readonly int _experationTime;
        readonly IMemoryCache _memoryCache;

        public RefreshTokenService(
            IConfiguration configuration,
            IMemoryCache memoryCache
        )
        {
            _experationTime = int.Parse(configuration["Jwt:RefreshToken:Lifetime"]!);
            _memoryCache = memoryCache;
        }

        public Task AddRefreshToken(
            Guid userId,
            string refreshToken,
            CancellationToken cancellationToken
        )
        {
            _memoryCache.Set(
                userId,
                refreshToken,
                DateTimeOffset.UtcNow.AddDays(_experationTime)
            );

            return Task.CompletedTask;
        }

        public Task RevokeRefreshToken(
            Guid userId,
            string refreshToken,
            CancellationToken cancellationToken
        )
        {
            if (_memoryCache.TryGetValue(userId, out string? token) && (token?.Equals(refreshToken) ?? false))
            {
                _memoryCache.Remove(userId);
            }

            return Task.CompletedTask;
        }

        public Task<bool> ValidateRefreshToken(
            Guid userId,
            string refreshToken,
            CancellationToken cancellationToken
        )
        {
            if (_memoryCache.TryGetValue(userId, out string? token))
            {
                return Task.FromResult(refreshToken.Equals(token));
            }

            return Task.FromResult(false);
        }
    }
}

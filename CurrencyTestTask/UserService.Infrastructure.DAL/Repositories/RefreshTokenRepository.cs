using Microsoft.EntityFrameworkCore;
using UserService.Application.Repositories;
using UserService.Domain.Entities;
using UserService.Infrastructure.DAL.Contexts;

namespace UserService.Infrastructure.DAL.Repositories
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        readonly UserServiceContext _userServiceContext;

        public RefreshTokenRepository(UserServiceContext userServiceContext)
        {
            _userServiceContext = userServiceContext;
        }

        public void Add(RefreshToken refreshToken)
        {
            _userServiceContext.RefreshTokens.Add(refreshToken);
        }

        public Task<RefreshToken?> Get(Guid userId, string hashToken, CancellationToken cancellationToken)
        {
            return _userServiceContext.RefreshTokens
                .Where(x => !x.IsRevoked)
                .Where(x => x.Expires >= DateTime.UtcNow)
                .Where(x => x.UserId == userId)
                .Where(x => x.HashToken == hashToken)
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}

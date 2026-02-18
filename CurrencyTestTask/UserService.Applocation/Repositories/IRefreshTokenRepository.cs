using UserService.Domain.Entities;

namespace UserService.Application.Repositories
{
    public interface IRefreshTokenRepository
    {
        Task<RefreshToken?> Get(
            Guid userId,
            string hashToken,
            CancellationToken cancellationToken
        );

        void Add(RefreshToken refreshToken);
    }
}

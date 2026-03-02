namespace UserService.Application.Interfaces
{
    public interface IRefreshTokenService
    {
        Task AddRefreshToken(
            Guid userId,
            string refreshToken,
            CancellationToken cancellationToken
        );

        Task<bool> ValidateRefreshToken(
            Guid userId,
            string refreshToken,
            CancellationToken cancellationToken
        );

        Task RevokeRefreshToken(
            Guid userId,
            string refreshToken,
            CancellationToken cancellationToken
        );
    }
}

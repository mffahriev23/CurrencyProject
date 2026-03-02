using MediatR;
using UserService.Application.Interfaces;

namespace UserService.Application.Users.Commands.LogOutV2
{
    public class LogOutCommandHandler : IRequestHandler<LogOutCommand>
    {
        readonly IRefreshTokenService _refreshTokenService;
        public LogOutCommandHandler(IRefreshTokenService refreshTokenService)
        {
            _refreshTokenService = refreshTokenService;
        }

        public async Task Handle(LogOutCommand request, CancellationToken cancellationToken)
        {
            await _refreshTokenService.RevokeRefreshToken(
                request.UserId,
                request.RefreshToken,
                cancellationToken
            );
        }
    }
}

using MediatR;
using UserService.Application.Interfaces;
using UserService.Application.Repositories;
using UserService.Application.UnitOfWork;
using UserService.Domain.Entities;

namespace UserService.Application.Users.Commands.LogOut
{
    public class LogOutCommandHandler : IRequestHandler<LogOutCommand>
    {
        readonly IUnitOfWork _unitOfWork;
        readonly IRefreshTokenRepository _refreshTokenRepository;

        public LogOutCommandHandler(
            IUnitOfWork unitOfWork,
            IRefreshTokenRepository refreshTokenRepository
        )
        {
            _unitOfWork = unitOfWork;
            _refreshTokenRepository = refreshTokenRepository;
        }

        public async Task Handle(LogOutCommand request, CancellationToken cancellationToken)
        {
            RefreshToken refreshToken = await _refreshTokenRepository.Get(
                request.UserId,
                request.RefreshToken,
                cancellationToken
            );

            refreshToken.IsRevoked = true;

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}

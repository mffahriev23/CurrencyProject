using Application.UnitOfWork;
using Authorization.Interfaces;
using MediatR;
using UserService.Application.Interfaces;
using UserService.Application.Repositories;
using UserService.Domain.Entities;

namespace UserService.Application.Users.Commands.Refresh
{
    public class RefreshCommandHandler : IRequestHandler<RefreshCommand, RefreshResult>
    {
        readonly IHasher _hasher;
        readonly IUserRepository _userRepository;
        readonly IRefreshTokenRepository _refreshTokenRepository;
        readonly IUnitOfWork _unitOfWork;
        readonly IJwtFactory _jwtManager;

        public RefreshCommandHandler(
            IHasher hasher,
            IUserRepository userRepository,
            IRefreshTokenRepository refreshTokenRepository,
            IUnitOfWork unitOfWork,
            IJwtFactory jwtManager
        )
        {
            _hasher = hasher;
            _userRepository = userRepository;
            _refreshTokenRepository = refreshTokenRepository;
            _unitOfWork = unitOfWork;
            _jwtManager = jwtManager;
        }

        public async Task<RefreshResult> Handle(
            RefreshCommand request,
            CancellationToken cancellationToken
        )
        {
            User? user = await _userRepository.GetUser(
                request.UserId,
                cancellationToken
            )
                ?? throw new ArgumentNullException("Пользователь не наден.");

            RefreshToken? oldRefreshToken = await _refreshTokenRepository.Get(
                request.UserId,
                request.RefreshToken,
                cancellationToken
            )
                ?? throw new ArgumentException("Токен обновления в бд не найден, либо недоступен.");

            oldRefreshToken.IsRevoked = true;

            string newRefreshTokenText = Guid.NewGuid().ToString();
            string accessToken = _jwtManager.GetJwtToken(user.Id, user.Name);

            RefreshToken newRefreshToken = new()
            {
                Id = Guid.NewGuid(),
                Created = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddDays(7),
                UserId = user.Id,
                HashToken = newRefreshTokenText,
                IsRevoked = false
            };

            _refreshTokenRepository.Add(newRefreshToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new RefreshResult(accessToken, newRefreshTokenText);
        }
    }
}

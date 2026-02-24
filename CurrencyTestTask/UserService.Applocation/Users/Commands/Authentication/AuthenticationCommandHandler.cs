using Application.UnitOfWork;
using Authorization.Interfaces;
using MediatR;
using UserService.Application.Interfaces;
using UserService.Application.Repositories;
using UserService.Domain.Entities;

namespace UserService.Application.Users.Commands.Authentication
{
    public class AuthenticationCommandHandler : IRequestHandler<AuthenticationCommand, AuthenticationResult>
    {
        readonly IHasher _hasher;
        readonly IUserRepository _userRepository;
        readonly IRefreshTokenRepository _refreshTokenRepository;
        readonly IUnitOfWork _unitOfWork;
        readonly IJwtFactory _jwtManager;

        public AuthenticationCommandHandler(
            IHasher passwordHasher,
            IUserRepository userRepository,
            IRefreshTokenRepository refreshTokenRepository,
            IUnitOfWork unitOfWork,
            IJwtFactory jwtManager
        )
        {
            _hasher = passwordHasher;
            _userRepository = userRepository;
            _refreshTokenRepository = refreshTokenRepository;
            _unitOfWork = unitOfWork;
            _jwtManager = jwtManager;
        }

        public async Task<AuthenticationResult> Handle(AuthenticationCommand request, CancellationToken cancellationToken)
        {
            User? user = await _userRepository.GetUser(request.Name, cancellationToken)
                ?? throw new ArgumentException($"Пользователь с именем {request.Name} не найден.");

            if (!_hasher.VerifyText(user.Password, request.Password))
            {
                throw new ArgumentException($"Введён некорректный пароль.");
            }

            string refreshTokenText = Guid.NewGuid().ToString();
            string accessToken = _jwtManager.GetJwtToken(user.Id, user.Name);

            RefreshToken refreshToken = new()
            {
                Id = Guid.NewGuid(),
                Created = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddDays(7),
                UserId = user.Id,
                HashToken = refreshTokenText,
                IsRevoked = false
            };

            _refreshTokenRepository.Add(refreshToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new AuthenticationResult(accessToken, refreshTokenText);
        }
    }
}

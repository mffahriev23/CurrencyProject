using Application.UnitOfWork;
using Application.Exceptions;
using Application.Interfaces;
using Application.Options;
using MediatR;
using Microsoft.Extensions.Options;
using UserService.Application.Interfaces;
using UserService.Application.Repositories;
using UserService.Domain.Entities;

namespace UserService.Application.Users.Commands.Authentication
{
    public class AuthenticationCommandHandler : IRequestHandler<AuthenticationCommand, AuthenticationResult>
    {
        readonly int _refreshTokenExpiration;
        readonly IHasher _hasher;
        readonly IUserRepository _userRepository;
        readonly IRefreshTokenRepository _refreshTokenRepository;
        readonly IUnitOfWork _unitOfWork;
        readonly IJwtFactory _jwtManager;

        public AuthenticationCommandHandler(
            IOptions<JwtManagerOptions> jwtManagerOptions,
            IHasher passwordHasher,
            IUserRepository userRepository,
            IRefreshTokenRepository refreshTokenRepository,
            IUnitOfWork unitOfWork,
            IJwtFactory jwtManager
        )
        {
            _refreshTokenExpiration = jwtManagerOptions.Value.ExpirationRefreshTokenOnDay!.Value;
            _hasher = passwordHasher;
            _userRepository = userRepository;
            _refreshTokenRepository = refreshTokenRepository;
            _unitOfWork = unitOfWork;
            _jwtManager = jwtManager;
        }

        public async Task<AuthenticationResult> Handle(AuthenticationCommand request, CancellationToken cancellationToken)
        {
            User? user = await _userRepository.GetUser(request.Name, cancellationToken)
                ?? throw new BadRequestException($"Пользователь с именем {request.Name} не найден.");

            if (!_hasher.VerifyText(user.Password, request.Password))
            {
                throw new BadRequestException($"Введён некорректный пароль.");
            }

            Guid refreshTokenKey = Guid.NewGuid();
            string refreshTokenText = _jwtManager.GetJwtToken(refreshTokenKey);
            string accessToken = _jwtManager.GetJwtToken(user.Id, user.Name);

            RefreshToken refreshToken = new()
            {
                Id = Guid.NewGuid(),
                Created = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddDays(_refreshTokenExpiration),
                UserId = user.Id,
                HashToken = refreshTokenKey.ToString(),
                IsRevoked = false
            };

            _refreshTokenRepository.Add(refreshToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new AuthenticationResult(accessToken, refreshTokenText);
        }
    }
}

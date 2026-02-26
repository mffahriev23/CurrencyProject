using System.Security.Claims;
using Application.UnitOfWork;
using Application.Exceptions;
using Application.Extensions;
using Application.Interfaces;
using Application.Options;
using MediatR;
using Microsoft.Extensions.Options;
using UserService.Application.Interfaces;
using UserService.Application.Repositories;
using UserService.Domain.Entities;

namespace UserService.Application.Users.Commands.Refresh
{
    public class RefreshCommandHandler : IRequestHandler<RefreshCommand, RefreshResult>
    {
        readonly int _refreshTokenExpiration;
        readonly IJwtReader _jwtReader;
        readonly IUserRepository _userRepository;
        readonly IRefreshTokenRepository _refreshTokenRepository;
        readonly IUnitOfWork _unitOfWork;
        readonly IJwtFactory _jwtManager;

        public RefreshCommandHandler(
            IOptions<JwtManagerOptions> jwtManagerOptions,
            IJwtReader jwtReader,
            IUserRepository userRepository,
            IRefreshTokenRepository refreshTokenRepository,
            IUnitOfWork unitOfWork,
            IJwtFactory jwtManager
        )
        {
            _refreshTokenExpiration = jwtManagerOptions.Value.ExpirationRefreshTokenOnDay!.Value;
            _jwtReader = jwtReader;
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
                ?? throw new BadRequestException("Пользователь не наден.");

            Guid refreshKey = _jwtReader.GetRefreshTokenClaims(request.RefreshToken)
                .GetRefreshKey();

            RefreshToken? oldRefreshToken = await _refreshTokenRepository.Get(
                request.UserId,
                refreshKey.ToString(),
                cancellationToken
            )
                ?? throw new BadRequestException("Токен обновления в бд не найден, либо недоступен.");

            oldRefreshToken.IsRevoked = true;

            Guid newRefreshKey = Guid.NewGuid();
            string newRefreshTokenText = _jwtManager.GetJwtToken(newRefreshKey);
            string accessToken = _jwtManager.GetJwtToken(user.Id, user.Name);

            RefreshToken newRefreshToken = new()
            {
                Id = Guid.NewGuid(),
                Created = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddDays(_refreshTokenExpiration),
                UserId = user.Id,
                HashToken = newRefreshKey.ToString(),
                IsRevoked = false
            };

            _refreshTokenRepository.Add(newRefreshToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new RefreshResult(accessToken, newRefreshTokenText);
        }
    }
}

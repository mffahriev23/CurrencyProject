using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Exceptions;
using MediatR;
using UserService.Application.Interfaces;
using UserService.Application.Repositories;
using UserService.Domain.Entities;

namespace UserService.Application.Users.Commands.AuthenticationV2
{
    public class AuthenticationCommandHandler : IRequestHandler<AuthenticationCommand, AuthenticationResult>
    {
        readonly IHasher _hasher;
        readonly IUserRepository _userRepository;
        readonly IRefreshTokenService _refreshTokenService;
        readonly ITokenGenerator _generator;

        public AuthenticationCommandHandler(
            IHasher passwordHasher,
            IUserRepository userRepository,
            IRefreshTokenService refreshTokenService,
            ITokenGenerator generator
        )
        {
            _hasher = passwordHasher;
            _userRepository = userRepository;
            _refreshTokenService = refreshTokenService;
            _generator = generator;
        }

        public async Task<AuthenticationResult> Handle(AuthenticationCommand request, CancellationToken cancellationToken)
        {
            User? user = await _userRepository.GetUser(request.Name, cancellationToken)
                ?? throw new BadRequestException($"Пользователь с именем {request.Name} не найден.");

            if (!_hasher.VerifyText(user.Password, request.Password))
            {
                throw new BadRequestException($"Введён некорректный пароль.");
            }

            string accessToken = _generator.GenerateAccessToken(user.Id, user.Name);
            string refreshToken = _generator.GenerateRefreshToken();

            await _refreshTokenService.AddRefreshToken(
                user.Id,
                refreshToken,
                cancellationToken
            );

            return new AuthenticationResult(accessToken, refreshToken);
        }
    }
}

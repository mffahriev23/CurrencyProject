using Application.Exceptions;
using Application.UnitOfWork;
using Microsoft.Extensions.Options;
using Moq;
using UserService.Application.Interfaces;
using UserService.Application.Repositories;
using UserService.Application.Services;
using UserService.Application.Users.Commands.AuthenticationV2;
using UserService.Domain.Entities;

namespace UserService.Tests.Handlers
{
    public class AuthenticationCommandHandlerTests
    {
        private readonly Mock<IHasher> _hasherMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IRefreshTokenService> _refreshTokenServiceMock;
        private readonly Mock<ITokenGenerator> _tokenGenerator;
        private readonly AuthenticationCommandHandler _handler;

        public AuthenticationCommandHandlerTests()
        {
            _hasherMock = new Mock<IHasher>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _refreshTokenServiceMock = new Mock<IRefreshTokenService>();
            _tokenGenerator = new Mock<ITokenGenerator>();

            _handler = new AuthenticationCommandHandler(
                _hasherMock.Object,
                _userRepositoryMock.Object,
                _refreshTokenServiceMock.Object,
                _tokenGenerator.Object
            );
        }

        [Fact]
        public async Task Handle_WhenUserExistsAndPasswordValid_ShouldReturnTokensAndAddRefreshToken()
        {
            string name = "user";
            string password = "password";
            Guid userId = Guid.NewGuid();

            User user = new()
            {
                Id = userId,
                Name = name,
                Password = "hashed"
            };

            _userRepositoryMock
                .Setup(x => x.GetUser(name, It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);

            _hasherMock
                .Setup(x => x.VerifyText(user.Password, password))
                .Returns(true);

            RefreshToken? captured = null;

            AuthenticationCommand command = new(name, password);

            AuthenticationResult result = await _handler.Handle(command, CancellationToken.None);

            Assert.Equal("access-token", result.AccessToken);
            Assert.False(string.IsNullOrWhiteSpace(result.RefreshToken));

            Assert.NotNull(captured);
            Assert.Equal(userId, captured!.UserId);
            Assert.Equal(result.RefreshToken, captured.HashToken);
            Assert.False(captured.IsRevoked);
            Assert.True(captured.Expires > captured.Created);
        }

        [Fact]
        public async Task Handle_WhenUserNotFound_ShouldThrowArgumentException()
        {
            _userRepositoryMock
                .Setup(x => x.GetUser(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((User?)null);

            AuthenticationCommand command = new("unknown", "pwd");

            await Assert.ThrowsAsync<BadRequestException>(
                () => _handler.Handle(command, CancellationToken.None)
            );
        }

        [Fact]
        public async Task Handle_WhenPasswordInvalid_ShouldThrowArgumentException()
        {
            string name = "user";
            string password = "password";

            User user = new()
            {
                Id = Guid.NewGuid(),
                Name = name,
                Password = "hashed"
            };

            _userRepositoryMock
                .Setup(x => x.GetUser(name, It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);

            _hasherMock
                .Setup(x => x.VerifyText(user.Password, password))
                .Returns(false);

            AuthenticationCommand command = new(name, password);

            await Assert.ThrowsAsync<BadRequestException>(
                () => _handler.Handle(command, CancellationToken.None)
            );
        }
    }
}


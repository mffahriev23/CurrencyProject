using Application.UnitOfWork;
using Authorization.Exceptions;
using Authorization.Interfaces;
using Authorization.Options;
using Microsoft.Extensions.Options;
using Moq;
using UserService.Application.Interfaces;
using UserService.Application.Repositories;
using UserService.Application.Users.Commands.Refresh;
using UserService.Domain.Entities;

namespace UserService.Tests.Handlers
{
    public class RefreshCommandHandlerTests
    {
        private readonly Mock<IJwtReader> _jwtReaderMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IRefreshTokenRepository> _refreshTokenRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IJwtFactory> _jwtManagerMock;
        private readonly Mock<IOptions<JwtManagerOptions>> _jwtManagerOptionsMock;
        private readonly RefreshCommandHandler _handler;

        public RefreshCommandHandlerTests()
        {
            _jwtReaderMock = new Mock<IJwtReader>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _refreshTokenRepositoryMock = new Mock<IRefreshTokenRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _jwtManagerMock = new Mock<IJwtFactory>();
            _jwtManagerOptionsMock = new Mock<IOptions<JwtManagerOptions>>();

            _handler = new RefreshCommandHandler(
                _jwtManagerOptionsMock.Object,
                _jwtReaderMock.Object,
                _userRepositoryMock.Object,
                _refreshTokenRepositoryMock.Object,
                _unitOfWorkMock.Object,
                _jwtManagerMock.Object
            );
        }

        [Fact]
        public async Task Handle_WhenUserAndTokenValid_ShouldRevokeOldAndReturnNewTokens()
        {
            Guid userId = Guid.NewGuid();
            string oldRefreshTokenText = "old-token";

            User user = new()
            {
                Id = userId,
                Name = "user",
                Password = "pwd"
            };

            RefreshToken oldToken = new()
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                HashToken = oldRefreshTokenText,
                Created = DateTime.UtcNow.AddDays(-1),
                Expires = DateTime.UtcNow.AddDays(1),
                IsRevoked = false
            };

            _userRepositoryMock
                .Setup(x => x.GetUser(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);

            _refreshTokenRepositoryMock
                .Setup(x => x.Get(userId, oldRefreshTokenText, It.IsAny<CancellationToken>()))
                .ReturnsAsync(oldToken);

            _jwtManagerMock
                .Setup(x => x.GetJwtToken(userId, user.Name))
                .Returns("new-access");

            RefreshToken? capturedNew = null;

            _refreshTokenRepositoryMock
                .Setup(x => x.Add(It.IsAny<RefreshToken>()))
                .Callback<RefreshToken>(t => capturedNew = t);

            _unitOfWorkMock
                .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            RefreshCommand command = new(userId, oldRefreshTokenText);

            RefreshResult result = await _handler.Handle(command, CancellationToken.None);

            Assert.True(oldToken.IsRevoked);
            Assert.Equal("new-access", result.AccessToken);
            Assert.False(string.IsNullOrWhiteSpace(result.RefreshToken));

            Assert.NotNull(capturedNew);
            Assert.Equal(userId, capturedNew!.UserId);
            Assert.Equal(result.RefreshToken, capturedNew.HashToken);
            Assert.False(capturedNew.IsRevoked);
        }

        [Fact]
        public async Task Handle_WhenUserNotFound_ShouldThrowArgumentNullException()
        {
            _userRepositoryMock
                .Setup(x => x.GetUser(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((User?)null);

            RefreshCommand command = new(Guid.NewGuid(), "token");

            await Assert.ThrowsAsync<BadRequestException>(
                () => _handler.Handle(command, CancellationToken.None)
            );
        }

        [Fact]
        public async Task Handle_WhenRefreshTokenNotFound_ShouldThrowArgumentException()
        {
            Guid userId = Guid.NewGuid();

            User user = new()
            {
                Id = userId,
                Name = "user",
                Password = "pwd"
            };

            _userRepositoryMock
                .Setup(x => x.GetUser(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);

            _refreshTokenRepositoryMock
                .Setup(x => x.Get(userId, It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((RefreshToken?)null);

            RefreshCommand command = new(userId, "token");

            await Assert.ThrowsAsync<BadRequestException>(
                () => _handler.Handle(command, CancellationToken.None)
            );
        }
    }
}


using Application.UnitOfWork;
using Moq;
using UserService.Application.Interfaces;
using UserService.Application.Users.Commands.LogOutV2;
using UserService.Domain.Entities;

namespace UserService.Tests.Handlers
{
    public class LogOutCommandHandlerTests
    {
        private readonly Mock<IRefreshTokenService> _refreshTokenServiceMock;
        private readonly LogOutCommandHandler _handler;

        public LogOutCommandHandlerTests()
        {
            _refreshTokenServiceMock = new Mock<IRefreshTokenService>();

            _handler = new LogOutCommandHandler(
                _refreshTokenServiceMock.Object
            );
        }

        [Fact]
        public async Task Handle_WhenRefreshTokenFound_ShouldRevokeTokenAndSaveChanges()
        {
            Guid userId = Guid.NewGuid();
            string refreshTokenText = "token";

            RefreshToken token = new()
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                HashToken = refreshTokenText,
                Created = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddDays(1),
                IsRevoked = false
            };

            _refreshTokenServiceMock
                .Setup(x => x.RevokeRefreshToken(userId, refreshTokenText, It.IsAny<CancellationToken>()));

            LogOutCommand command = new(userId, refreshTokenText);

            await _handler.Handle(command, CancellationToken.None);

            Assert.True(token.IsRevoked);
        }
    }
}


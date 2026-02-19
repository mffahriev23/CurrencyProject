using Moq;
using UserService.Application.Interfaces;
using UserService.Application.Repositories;
using UserService.Application.UnitOfWork;
using UserService.Application.Users.Commands.LogOut;
using UserService.Domain.Entities;

namespace UserService.Tests.Handlers
{
    public class LogOutCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IRefreshTokenRepository> _refreshTokenRepositoryMock;
        private readonly LogOutCommandHandler _handler;

        public LogOutCommandHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _refreshTokenRepositoryMock = new Mock<IRefreshTokenRepository>();

            _handler = new LogOutCommandHandler(
                _unitOfWorkMock.Object,
                _refreshTokenRepositoryMock.Object
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

            _refreshTokenRepositoryMock
                .Setup(x => x.Get(userId, refreshTokenText, It.IsAny<CancellationToken>()))
                .ReturnsAsync(token);

            _unitOfWorkMock
                .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            LogOutCommand command = new(userId, refreshTokenText);

            await _handler.Handle(command, CancellationToken.None);

            Assert.True(token.IsRevoked);
        }
    }
}


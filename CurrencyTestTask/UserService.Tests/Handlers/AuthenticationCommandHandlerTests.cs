using Authorization.Interfaces;
using Moq;
using UserService.Application.Interfaces;
using UserService.Application.Repositories;
using UserService.Application.UnitOfWork;
using UserService.Application.Users.Commands.Authentication;
using UserService.Domain.Entities;

namespace UserService.Tests.Handlers
{
    public class AuthenticationCommandHandlerTests
    {
        private readonly Mock<IHasher> _hasherMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IRefreshTokenRepository> _refreshTokenRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IJwtManager> _jwtManagerMock;
        private readonly AuthenticationCommandHandler _handler;

        public AuthenticationCommandHandlerTests()
        {
            _hasherMock = new Mock<IHasher>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _refreshTokenRepositoryMock = new Mock<IRefreshTokenRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _jwtManagerMock = new Mock<IJwtManager>();

            _handler = new AuthenticationCommandHandler(
                _hasherMock.Object,
                _userRepositoryMock.Object,
                _refreshTokenRepositoryMock.Object,
                _unitOfWorkMock.Object,
                _jwtManagerMock.Object
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

            _jwtManagerMock
                .Setup(x => x.GetJwtToken(userId, name))
                .Returns("access-token");

            RefreshToken? captured = null;

            _refreshTokenRepositoryMock
                .Setup(x => x.Add(It.IsAny<RefreshToken>()))
                .Callback<RefreshToken>(t => captured = t);

            _unitOfWorkMock
                .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

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

            await Assert.ThrowsAsync<ArgumentException>(
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

            await Assert.ThrowsAsync<ArgumentException>(
                () => _handler.Handle(command, CancellationToken.None)
            );
        }
    }
}


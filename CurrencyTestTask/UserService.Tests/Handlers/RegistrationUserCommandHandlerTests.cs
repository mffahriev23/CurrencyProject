using Application.UnitOfWork;
using Application.Exceptions;
using Moq;
using UserService.Application.Interfaces;
using UserService.Application.Repositories;
using UserService.Application.Users.Commands.Registration;
using UserService.Domain.Entities;

namespace UserService.Tests.Handlers
{
    public class RegistrationUserCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IHasher> _hasherMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly RegistrationUserCommandHandler _handler;

        public RegistrationUserCommandHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _hasherMock = new Mock<IHasher>();
            _userRepositoryMock = new Mock<IUserRepository>();

            _handler = new RegistrationUserCommandHandler(
                _unitOfWorkMock.Object,
                _hasherMock.Object,
                _userRepositoryMock.Object
            );
        }

        [Fact]
        public async Task Handle_WhenPasswordsDoNotMatch_ShouldThrowArgumentException()
        {
            RegistrationUserCommand command = new(
                "user",
                "password1",
                "password2"
            );

            await Assert.ThrowsAsync<BadRequestException>(
                () => _handler.Handle(command, CancellationToken.None)
            );
        }

        [Fact]
        public async Task Handle_WhenPasswordsMatch_ShouldAddUserAndSaveChanges()
        {
            string name = "user";
            string password = "password";
            string hashed = "hashed-password";

            _hasherMock
                .Setup(x => x.GetHash(password))
                .Returns(hashed);

            User? capturedUser = null;

            _userRepositoryMock
                .Setup(x => x.Add(It.IsAny<User>()))
                .Callback<User>(u => capturedUser = u);

            _unitOfWorkMock
                .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            RegistrationUserCommand command = new(
                name,
                password,
                password
            );

            await _handler.Handle(command, CancellationToken.None);

            Assert.NotNull(capturedUser);
            Assert.Equal(name, capturedUser!.Name);
            Assert.Equal(hashed, capturedUser.Password);
            Assert.NotEqual(Guid.Empty, capturedUser.Id);
        }
    }
}


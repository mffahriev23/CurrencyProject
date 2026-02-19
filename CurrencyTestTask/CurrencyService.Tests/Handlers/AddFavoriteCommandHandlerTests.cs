using Application.UnitOfWork;
using CurrencyService.Application.Favorites.Commands.AddFavorite;
using CurrencyService.Application.Repositories;
using CurrencyService.Domain.Entities;
using Moq;

namespace CurrencyService.Tests.Currency
{
    public class AddFavoriteCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IFavoriteRepository> _favoriteRepositoryMock;
        private readonly AddFavoriteCommandHandler _handler;

        public AddFavoriteCommandHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _favoriteRepositoryMock = new Mock<IFavoriteRepository>();
            _handler = new AddFavoriteCommandHandler(
                _unitOfWorkMock.Object,
                _favoriteRepositoryMock.Object
            );
        }

        [Fact]
        public async Task Handle_ValidCommand_ShouldCreateFavoritesAndSaveChanges()
        {
            Guid userId = Guid.NewGuid();
            Guid[] currencyIds =
            {
                Guid.NewGuid(),
                Guid.NewGuid()
            };

            AddFavoriteCommand command = new(
                currencyIds,
                userId
            );

            CancellationToken cancellationToken = CancellationToken.None;

            Favorite[]? capturedFavorites = null;

            _favoriteRepositoryMock
                .Setup(r => r.AddRange(It.IsAny<Favorite[]>()))
                .Callback<Favorite[]>(favorites => capturedFavorites = favorites);

            _unitOfWorkMock
                .Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            await _handler.Handle(
                command,
                cancellationToken
            );

            // Assert
            Assert.NotNull(capturedFavorites);
            Assert.Equal(
                currencyIds.Length,
                capturedFavorites!.Length
            );

            foreach (Favorite favorite in capturedFavorites)
            {
                Assert.Equal(
                    userId,
                    favorite.UserId
                );

                Assert.Contains(
                    favorite.CurrencyId,
                    currencyIds
                );

                Assert.True(favorite.IsActive);
            }
        }

        [Fact]
        public async Task Handle_EmptyCurrencyIds_ShouldCallAddRangeWithEmptyArrayAndSaveChanges()
        {
            Guid userId = Guid.NewGuid();
            Guid[] currencyIds = Array.Empty<Guid>();

            AddFavoriteCommand command = new(
                currencyIds,
                userId
            );

            CancellationToken cancellationToken = CancellationToken.None;

            Favorite[]? capturedFavorites = null;

            _favoriteRepositoryMock
                .Setup(r => r.AddRange(It.IsAny<Favorite[]>()))
                .Callback<Favorite[]>(favorites => capturedFavorites = favorites);

            _unitOfWorkMock
                .Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            await _handler.Handle(
                command,
                cancellationToken
            );

            Assert.NotNull(capturedFavorites);
            Assert.Empty(capturedFavorites!);
        }

        [Fact]
        public async Task Handle_WhenSaveChangesThrows_ShouldPropagateException()
        {
            Guid userId = Guid.NewGuid();
            Guid[] currencyIds =
            {
                Guid.NewGuid()
            };

            AddFavoriteCommand command = new(
                currencyIds,
                userId
            );

            CancellationToken cancellationToken = CancellationToken.None;

            Exception expectedException = new("SaveChanges failed");

            _favoriteRepositoryMock
                .Setup(r => r.AddRange(It.IsAny<Favorite[]>()))
                .Verifiable();

            _unitOfWorkMock
                .Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ThrowsAsync(expectedException);

            await Assert.ThrowsAsync<Exception>(
                () => _handler.Handle(
                    command,
                    cancellationToken
                )
            );
        }

        [Fact]
        public async Task Handle_WhenCancellationRequested_ShouldPassCancellationTokenToSaveChanges()
        {
            Guid userId = Guid.NewGuid();
            Guid[] currencyIds =
            {
                Guid.NewGuid()
            };

            AddFavoriteCommand command = new(
                currencyIds,
                userId
            );

            using CancellationTokenSource cts = new();
            cts.Cancel();
            CancellationToken cancellationToken = cts.Token;

            _favoriteRepositoryMock
                .Setup(r => r.AddRange(It.IsAny<Favorite[]>()))
                .Verifiable();

            _unitOfWorkMock
                .Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ThrowsAsync(new OperationCanceledException());

            await Assert.ThrowsAsync<OperationCanceledException>(
                () => _handler.Handle(
                    command,
                    cancellationToken
                )
            );
        }
    }
}


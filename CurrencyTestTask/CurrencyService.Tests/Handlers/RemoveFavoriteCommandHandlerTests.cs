using Application.UnitOfWork;
using CurrencyService.Application.Favorites.Commands.RemoveFavorite;
using CurrencyService.Application.Repositories;
using CurrencyService.Domain.Entities;
using Moq;

namespace CurrencyService.Tests.Currency
{
    public class RemoveFavoriteCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IFavoriteRepository> _favoriteRepositoryMock;
        private readonly RemoveFavoriteCommandHandler _handler;

        public RemoveFavoriteCommandHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _favoriteRepositoryMock = new Mock<IFavoriteRepository>();
            _handler = new RemoveFavoriteCommandHandler(
                _unitOfWorkMock.Object,
                _favoriteRepositoryMock.Object
            );
        }

        [Fact]
        public async Task Handle_ValidCommand_ShouldDeleteFavoritesAndSaveChanges()
        {
            Guid userId = Guid.NewGuid();
            Guid currencyId1 = Guid.NewGuid();
            Guid currencyId2 = Guid.NewGuid();

            Favorite favorite1 = Favorite.Create(DateTime.UtcNow, userId, currencyId1);
            Favorite favorite2 = Favorite.Create(DateTime.UtcNow, userId, currencyId2);
            Favorite[] favorites = { favorite1, favorite2 };

            RemoveFavoriteCommand command = new(
                new[] { currencyId1, currencyId2 },
                userId
            );

            CancellationToken cancellationToken = CancellationToken.None;

            _favoriteRepositoryMock
                .Setup(r => r.GetByUserId(userId, cancellationToken))
                .ReturnsAsync(favorites);

            _unitOfWorkMock
                .Setup(u => u.SaveChangesAsync(cancellationToken))
                .ReturnsAsync(1);

            await _handler.Handle(command, cancellationToken);

            Assert.False(favorite1.IsActive);
            Assert.False(favorite2.IsActive);
        }

        [Fact]
        public async Task Handle_WhenFavoriteNotFound_ShouldContinueAndSaveChanges()
        {
            Guid userId = Guid.NewGuid();
            Guid existingCurrencyId = Guid.NewGuid();
            Guid nonExistingCurrencyId = Guid.NewGuid();

            Favorite existingFavorite = Favorite.Create(DateTime.UtcNow, userId, existingCurrencyId);
            Favorite[] favorites = { existingFavorite };

            RemoveFavoriteCommand command = new(
                new[] { existingCurrencyId, nonExistingCurrencyId },
                userId
            );

            CancellationToken cancellationToken = CancellationToken.None;

            _favoriteRepositoryMock
                .Setup(r => r.GetByUserId(userId, cancellationToken))
                .ReturnsAsync(favorites);

            _unitOfWorkMock
                .Setup(u => u.SaveChangesAsync(cancellationToken))
                .ReturnsAsync(1);

            await _handler.Handle(command, cancellationToken);

            Assert.False(existingFavorite.IsActive);
        }

        [Fact]
        public async Task Handle_EmptyCurrencyIds_ShouldNotDeleteAnythingAndSaveChanges()
        {
            Guid userId = Guid.NewGuid();
            Guid currencyId = Guid.NewGuid();

            Favorite favorite = Favorite.Create(DateTime.UtcNow, userId, currencyId);
            Favorite[] favorites = { favorite };

            RemoveFavoriteCommand command = new(
                Array.Empty<Guid>(),
                userId
            );

            CancellationToken cancellationToken = CancellationToken.None;

            _favoriteRepositoryMock
                .Setup(r => r.GetByUserId(userId, cancellationToken))
                .ReturnsAsync(favorites);

            _unitOfWorkMock
                .Setup(u => u.SaveChangesAsync(cancellationToken))
                .ReturnsAsync(1);

            await _handler.Handle(command, cancellationToken);

            Assert.True(favorite.IsActive);
        }

        [Fact]
        public async Task Handle_WhenSaveChangesThrows_ShouldPropagateException()
        {
            Guid userId = Guid.NewGuid();
            Guid currencyId = Guid.NewGuid();

            Favorite favorite = Favorite.Create(DateTime.UtcNow, userId, currencyId);
            Favorite[] favorites = { favorite };

            RemoveFavoriteCommand command = new(
                new[] { currencyId },
                userId
            );

            CancellationToken cancellationToken = CancellationToken.None;
            Exception expectedException = new("SaveChanges failed");

            _favoriteRepositoryMock
                .Setup(r => r.GetByUserId(userId, cancellationToken))
                .ReturnsAsync(favorites);

            _unitOfWorkMock
                .Setup(u => u.SaveChangesAsync(cancellationToken))
                .ThrowsAsync(expectedException);

            await Assert.ThrowsAsync<Exception>(
                () => _handler.Handle(command, cancellationToken)
            );
        }

        [Fact]
        public async Task Handle_WhenRepositoryReturnsEmptyArray_ShouldNotDeleteAnythingAndSaveChanges()
        {
            Guid userId = Guid.NewGuid();
            Guid currencyId = Guid.NewGuid();

            Favorite[] favorites = Array.Empty<Favorite>();

            RemoveFavoriteCommand command = new(
                new[] { currencyId },
                userId
            );

            CancellationToken cancellationToken = CancellationToken.None;

            _favoriteRepositoryMock
                .Setup(r => r.GetByUserId(userId, cancellationToken))
                .ReturnsAsync(favorites);

            _unitOfWorkMock
                .Setup(u => u.SaveChangesAsync(cancellationToken))
                .ReturnsAsync(1);

            await _handler.Handle(command, cancellationToken);
        }

        [Fact]
        public async Task Handle_WhenRepositoryThrows_ShouldPropagateException()
        {
            Guid userId = Guid.NewGuid();
            Guid currencyId = Guid.NewGuid();

            RemoveFavoriteCommand command = new(
                new[] { currencyId },
                userId
            );

            CancellationToken cancellationToken = CancellationToken.None;
            Exception expectedException = new("Repository error");

            _favoriteRepositoryMock
                .Setup(r => r.GetByUserId(userId, cancellationToken))
                .ThrowsAsync(expectedException);

            await Assert.ThrowsAsync<Exception>(
                () => _handler.Handle(command, cancellationToken)
            );
        }
    }
}

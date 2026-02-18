using CurrencyService.Application.Favorites.Notifications;
using CurrencyService.Application.Repositories;
using CurrencyService.Domain.Entities;
using CurrencyService.Domain.Enums;
using CurrencyService.Domain.Events;
using Moq;

namespace CurrencyService.Tests.Handlers
{
    public class DeletedFavoriteCurrencyHandlerTests
    {
        private readonly Mock<IHistoryRepository> _historyRepositoryMock;
        private readonly DeletedFavoriteCurrencyHandler _handler;

        public DeletedFavoriteCurrencyHandlerTests()
        {
            _historyRepositoryMock = new Mock<IHistoryRepository>();
            _handler = new DeletedFavoriteCurrencyHandler(_historyRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldAddHistoryWithDeleteAction()
        {
            Guid userId = Guid.NewGuid();
            Guid currencyId = Guid.NewGuid();
            DateTime timestamp = DateTime.UtcNow;

            Favorite favorite = Favorite.Create(timestamp, userId, currencyId);

            DeletedFavoriteCurrencyEvent notification = new(
                timestamp,
                favorite
            );

            FavoriteHistory? captured = null;

            _historyRepositoryMock
                .Setup(x => x.Add(It.IsAny<FavoriteHistory>()))
                .Callback<FavoriteHistory>(h => captured = h);

            await _handler.Handle(notification, CancellationToken.None);

            Assert.NotNull(captured);
            Assert.Equal(favorite.Id, captured!.FavoriteId);
            Assert.Equal(FavoriteActions.Delete, captured.Action);
            Assert.Equal(timestamp, captured.Created);
        }
    }
}


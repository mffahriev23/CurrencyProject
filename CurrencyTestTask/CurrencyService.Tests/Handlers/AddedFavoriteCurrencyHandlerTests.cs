using CurrencyService.Application.Favorites.Notifications;
using CurrencyService.Application.Repositories;
using CurrencyService.Domain.Entities;
using CurrencyService.Domain.Enums;
using CurrencyService.Domain.Events;
using Moq;

namespace CurrencyService.Tests.Handlers
{
    public class AddedFavoriteCurrencyHandlerTests
    {
        private readonly Mock<IHistoryRepository> _historyRepositoryMock;
        private readonly AddedFavoriteCurrencyHandler _handler;

        public AddedFavoriteCurrencyHandlerTests()
        {
            _historyRepositoryMock = new Mock<IHistoryRepository>();
            _handler = new AddedFavoriteCurrencyHandler(_historyRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldAddHistoryWithCreateAction()
        {
            Guid userId = Guid.NewGuid();
            Guid currencyId = Guid.NewGuid();
            DateTime timestamp = DateTime.UtcNow;

            Favorite favorite = Favorite.Create(timestamp, userId, currencyId);

            AddedFavoriteCurrencyEvent notification = new(
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
            Assert.Equal(FavoriteActions.Create, captured.Action);
            Assert.Equal(timestamp, captured.Created);
        }
    }
}


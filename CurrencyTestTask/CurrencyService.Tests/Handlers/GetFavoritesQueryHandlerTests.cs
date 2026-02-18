using CurrencyService.Application.Favorites.Queries.GetFavorites;
using CurrencyService.Application.Repositories;
using CurrencyService.Domain.Entities;
using CurrencyEntity = CurrencyService.Domain.Entities.Currency;
using Moq;
using System.Reflection;

namespace CurrencyService.Tests.Currency
{
    public class GetFavoritesQueryHandlerTests
    {
        private readonly Mock<IFavoriteRepository> _favoriteRepositoryMock;
        private readonly GetFavoritesQueryHandler _handler;

        public GetFavoritesQueryHandlerTests()
        {
            _favoriteRepositoryMock = new Mock<IFavoriteRepository>();
            _handler = new GetFavoritesQueryHandler(_favoriteRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ValidQuery_ShouldReturnMappedFavoriteItems()
        {
            Guid userId = Guid.NewGuid();
            Guid currencyId1 = Guid.NewGuid();
            Guid currencyId2 = Guid.NewGuid();

            CurrencyEntity currency1 = new CurrencyEntity("USD", 75.50m);
            CurrencyEntity currency2 = new CurrencyEntity("EUR", 82.30m);

            Favorite favorite1 = Favorite.Create(DateTime.UtcNow, userId, currencyId1);
            Favorite favorite2 = Favorite.Create(DateTime.UtcNow, userId, currencyId2);

            SetCurrency(favorite1, currency1);
            SetCurrency(favorite2, currency2);

            Favorite[] favorites = { favorite1, favorite2 };

            GetFavoritesQuery query = new(userId);
            CancellationToken cancellationToken = CancellationToken.None;

            _favoriteRepositoryMock
                .Setup(r => r.GetByUserIdNoTracking(userId, cancellationToken))
                .ReturnsAsync(favorites);

            FavoriteItem[] result = await _handler.Handle(query, cancellationToken);

            Assert.NotNull(result);
            Assert.Equal(2, result.Length);
            Assert.Equal(favorite1.Id, result[0].Id);
            Assert.Equal(currency1.Id, result[0].CurrencyId);
            Assert.Equal(currency1.Name, result[0].Name);
            Assert.Equal(currency1.Rate, result[0].Rate);
            Assert.Equal(favorite2.Id, result[1].Id);
            Assert.Equal(currency2.Id, result[1].CurrencyId);
            Assert.Equal(currency2.Name, result[1].Name);
            Assert.Equal(currency2.Rate, result[1].Rate);
        }

        [Fact]
        public async Task Handle_EmptyFavorites_ShouldReturnEmptyArray()
        {
            Guid userId = Guid.NewGuid();
            Favorite[] favorites = Array.Empty<Favorite>();

            GetFavoritesQuery query = new(userId);
            CancellationToken cancellationToken = CancellationToken.None;

            _favoriteRepositoryMock
                .Setup(r => r.GetByUserIdNoTracking(userId, cancellationToken))
                .ReturnsAsync(favorites);

            FavoriteItem[] result = await _handler.Handle(query, cancellationToken);

            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task Handle_SingleFavorite_ShouldReturnSingleItem()
        {
            Guid userId = Guid.NewGuid();
            Guid currencyId = Guid.NewGuid();

            CurrencyEntity currency = new CurrencyEntity("GBP", 95.20m);
            Favorite favorite = Favorite.Create(DateTime.UtcNow, userId, currencyId);

            SetCurrency(favorite, currency);

            Favorite[] favorites = { favorite };

            GetFavoritesQuery query = new(userId);
            CancellationToken cancellationToken = CancellationToken.None;

            _favoriteRepositoryMock
                .Setup(r => r.GetByUserIdNoTracking(userId, cancellationToken))
                .ReturnsAsync(favorites);

            FavoriteItem[] result = await _handler.Handle(query, cancellationToken);

            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(favorite.Id, result[0].Id);
            Assert.Equal(currency.Id, result[0].CurrencyId);
            Assert.Equal(currency.Name, result[0].Name);
            Assert.Equal(currency.Rate, result[0].Rate);
        }

        [Fact]
        public async Task Handle_WhenRepositoryThrows_ShouldPropagateException()
        {
            Guid userId = Guid.NewGuid();
            GetFavoritesQuery query = new(userId);
            CancellationToken cancellationToken = CancellationToken.None;
            Exception expectedException = new("Repository error");

            _favoriteRepositoryMock
                .Setup(r => r.GetByUserIdNoTracking(userId, cancellationToken))
                .ThrowsAsync(expectedException);

            await Assert.ThrowsAsync<Exception>(
                () => _handler.Handle(query, cancellationToken)
            );
        }

        private static void SetCurrency(Favorite favorite, CurrencyEntity currency)
        {
            FieldInfo? currencyField = typeof(Favorite)
                .GetField("_currency", BindingFlags.NonPublic | BindingFlags.Instance);

            currencyField?.SetValue(favorite, currency);
        }
    }
}

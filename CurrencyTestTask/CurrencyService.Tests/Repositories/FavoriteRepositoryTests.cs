using CurrentService.Infrastructure.DAL.Contexts;
using CurrentService.Infrastructure.DAL.Repositories;
using CurrencyService.Domain.Entities;
using CurrencyEntity = CurrencyService.Domain.Entities.Currency;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Reflection;

namespace CurrencyService.Tests.Repositories
{
    public class FavoriteRepositoryTests : IDisposable
    {
        private readonly CurrencyServiceContext _context;
        private readonly FavoriteRepository _repository;
        private readonly Mock<IMediator> _mediatorMock;

        public FavoriteRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<CurrencyServiceContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _mediatorMock = new Mock<IMediator>();
            _context = new CurrencyServiceContext(options, _mediatorMock.Object);
            _repository = new FavoriteRepository(_context);
        }

        [Fact]
        public async Task GetByUserId_WhenFavoritesExist_ShouldReturnAllFavorites()
        {
            Guid userId = Guid.NewGuid();

            CurrencyEntity currency1 = new CurrencyEntity("USD", 75.50m);
            CurrencyEntity currency2 = new CurrencyEntity("EUR", 82.30m);

            _context.Currencies.AddRange(currency1, currency2);
            await _context.SaveChangesAsync();

            Favorite favorite1 = Favorite.Create(DateTime.UtcNow, userId, currency1.Id);
            Favorite favorite2 = Favorite.Create(DateTime.UtcNow, userId, currency2.Id);

            SetCurrency(favorite1, currency1);
            SetCurrency(favorite2, currency2);

            _context.Favorites.AddRange(favorite1, favorite2);
            await _context.SaveChangesAsync();

            CancellationToken cancellationToken = CancellationToken.None;

            Favorite[] result = await _repository.GetByUserId(userId, cancellationToken);

            Assert.Equal(2, result.Length);
            Assert.Contains(result, x => x.CurrencyId == currency1.Id);
            Assert.Contains(result, x => x.CurrencyId == currency2.Id);
        }

        [Fact]
        public async Task GetByUserId_WhenNoFavorites_ShouldReturnEmptyArray()
        {
            Guid userId = Guid.NewGuid();
            CancellationToken cancellationToken = CancellationToken.None;

            Favorite[] result = await _repository.GetByUserId(userId, cancellationToken);

            Assert.Empty(result);
        }

        [Fact]
        public async Task GetByUserId_ShouldOnlyReturnFavoritesForSpecifiedUser()
        {
            Guid userId1 = Guid.NewGuid();
            Guid userId2 = Guid.NewGuid();

            CurrencyEntity currency = new CurrencyEntity("USD", 75.50m);
            _context.Currencies.Add(currency);
            await _context.SaveChangesAsync();

            Favorite favorite1 = Favorite.Create(DateTime.UtcNow, userId1, currency.Id);
            Favorite favorite2 = Favorite.Create(DateTime.UtcNow, userId2, currency.Id);

            SetCurrency(favorite1, currency);
            SetCurrency(favorite2, currency);

            _context.Favorites.AddRange(favorite1, favorite2);
            await _context.SaveChangesAsync();

            CancellationToken cancellationToken = CancellationToken.None;

            Favorite[] result = await _repository.GetByUserId(userId1, cancellationToken);

            Assert.Single(result);
            Assert.Equal(userId1, result[0].UserId);
        }

        [Fact]
        public async Task GetByUserIdNoTracking_WhenFavoritesExist_ShouldReturnActiveFavorites()
        {
            Guid userId = Guid.NewGuid();

            CurrencyEntity currency1 = new CurrencyEntity("USD", 75.50m);
            CurrencyEntity currency2 = new CurrencyEntity("EUR", 82.30m);

            _context.Currencies.AddRange(currency1, currency2);
            await _context.SaveChangesAsync();

            Favorite favorite1 = Favorite.Create(DateTime.UtcNow, userId, currency1.Id);
            Favorite favorite2 = Favorite.Create(DateTime.UtcNow, userId, currency2.Id);
            favorite2.Delete(DateTime.UtcNow);

            SetCurrency(favorite1, currency1);
            SetCurrency(favorite2, currency2);

            _context.Favorites.AddRange(favorite1, favorite2);
            await _context.SaveChangesAsync();

            CancellationToken cancellationToken = CancellationToken.None;

            Favorite[] result = await _repository.GetByUserIdNoTracking(userId, cancellationToken);

            Assert.Single(result);
            Assert.True(result[0].IsActive);
            Assert.Equal(currency1.Id, result[0].CurrencyId);
        }

        [Fact]
        public async Task GetByUserIdNoTracking_WhenNoActiveFavorites_ShouldReturnEmptyArray()
        {
            Guid userId = Guid.NewGuid();

            CurrencyEntity currency = new CurrencyEntity("USD", 75.50m);
            _context.Currencies.Add(currency);
            await _context.SaveChangesAsync();

            Favorite favorite = Favorite.Create(DateTime.UtcNow, userId, currency.Id);
            favorite.Delete(DateTime.UtcNow);

            SetCurrency(favorite, currency);

            _context.Favorites.Add(favorite);
            await _context.SaveChangesAsync();

            CancellationToken cancellationToken = CancellationToken.None;

            Favorite[] result = await _repository.GetByUserIdNoTracking(userId, cancellationToken);

            Assert.Empty(result);
        }

        [Fact]
        public async Task GetByUserIdNoTracking_ShouldIncludeCurrency()
        {
            Guid userId = Guid.NewGuid();

            CurrencyEntity currency = new CurrencyEntity("USD", 75.50m);
            _context.Currencies.Add(currency);
            await _context.SaveChangesAsync();

            Favorite favorite = Favorite.Create(DateTime.UtcNow, userId, currency.Id);

            SetCurrency(favorite, currency);

            _context.Favorites.Add(favorite);
            await _context.SaveChangesAsync();

            CancellationToken cancellationToken = CancellationToken.None;

            Favorite[] result = await _repository.GetByUserIdNoTracking(userId, cancellationToken);

            Assert.Single(result);
            Assert.NotNull(result[0].Currency);
            Assert.Equal("USD", result[0].Currency!.Name);
        }

        [Fact]
        public async Task GetByUserId_ShouldIncludeCurrency()
        {
            Guid userId = Guid.NewGuid();

            CurrencyEntity currency = new CurrencyEntity("EUR", 82.30m);
            _context.Currencies.Add(currency);
            await _context.SaveChangesAsync();

            Favorite favorite = Favorite.Create(DateTime.UtcNow, userId, currency.Id);

            SetCurrency(favorite, currency);

            _context.Favorites.Add(favorite);
            await _context.SaveChangesAsync();

            CancellationToken cancellationToken = CancellationToken.None;

            Favorite[] result = await _repository.GetByUserId(userId, cancellationToken);

            Assert.Single(result);
            Assert.NotNull(result[0].Currency);
            Assert.Equal("EUR", result[0].Currency!.Name);
        }

        [Fact]
        public void AddRange_ShouldAddFavoritesToContext()
        {
            Guid userId = Guid.NewGuid();

            CurrencyEntity currency1 = new CurrencyEntity("USD", 75.50m);
            CurrencyEntity currency2 = new CurrencyEntity("EUR", 82.30m);

            Favorite favorite1 = Favorite.Create(DateTime.UtcNow, userId, currency1.Id);
            Favorite favorite2 = Favorite.Create(DateTime.UtcNow, userId, currency2.Id);

            Favorite[] favorites = { favorite1, favorite2 };

            _repository.AddRange(favorites);

            Assert.Equal(2, _context.Favorites.Local.Count);
            Assert.Contains(_context.Favorites.Local, x => x.CurrencyId == currency1.Id);
            Assert.Contains(_context.Favorites.Local, x => x.CurrencyId == currency2.Id);
        }

        [Fact]
        public void AddRange_WithEmptyArray_ShouldNotThrow()
        {
            Favorite[] favorites = Array.Empty<Favorite>();

            _repository.AddRange(favorites);

            Assert.Empty(_context.Favorites.Local);
        }

        private static void SetCurrency(Favorite favorite, CurrencyEntity currency)
        {
            FieldInfo? currencyField = typeof(Favorite)
                .GetField("_currency", BindingFlags.NonPublic | BindingFlags.Instance);

            currencyField?.SetValue(favorite, currency);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}

using CurrentService.Infrastructure.DAL.Contexts;
using CurrentService.Infrastructure.DAL.Repositories;
using CurrencyService.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace CurrencyService.Tests.Repositories
{
    public class HistoryRepositoryTests : IDisposable
    {
        private readonly CurrencyServiceContext _context;
        private readonly HistoryRepository _repository;
        private readonly Mock<IMediator> _mediatorMock;

        public HistoryRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<CurrencyServiceContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _mediatorMock = new Mock<IMediator>();
            _context = new CurrencyServiceContext(options, _mediatorMock.Object);
            _repository = new HistoryRepository(_context);
        }

        [Fact]
        public void Add_WhenCalled_ShouldTrackEntityInContext()
        {
            Guid favoriteId = Guid.NewGuid();
            DateTime created = DateTime.UtcNow;

            FavoriteHistory history = FavoriteHistory.CreateCreated(favoriteId, created);

            _repository.Add(history);

            Assert.Single(_context.Histories.Local);
            Assert.Equal(history.Id, _context.Histories.Local.Single().Id);
        }

        [Fact]
        public async Task Add_WhenSaved_ShouldPersistFavoriteHistory()
        {
            Guid favoriteId = Guid.NewGuid();
            DateTime created = DateTime.UtcNow;

            FavoriteHistory history = FavoriteHistory.CreateDeleted(favoriteId, created);

            _repository.Add(history);
            await _context.SaveChangesAsync();

            FavoriteHistory[] stored = await _context.Histories
                .AsNoTracking()
                .ToArrayAsync();

            Assert.Single(stored);
            Assert.Equal(history.Id, stored[0].Id);
            Assert.Equal(favoriteId, stored[0].FavoriteId);
            Assert.Equal(history.Action, stored[0].Action);
            Assert.Equal(created, stored[0].Created);
        }

        [Fact]
        public async Task Add_MultipleTimes_ShouldPersistAll()
        {
            Guid favoriteId = Guid.NewGuid();

            FavoriteHistory history1 = FavoriteHistory.CreateCreated(favoriteId, DateTime.UtcNow.AddMinutes(-5));
            FavoriteHistory history2 = FavoriteHistory.CreateDeleted(favoriteId, DateTime.UtcNow);

            _repository.Add(history1);
            _repository.Add(history2);
            await _context.SaveChangesAsync();

            int count = await _context.Histories.AsNoTracking().CountAsync();
            Assert.Equal(2, count);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}


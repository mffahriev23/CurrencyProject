using CurrentService.Infrastructure.DAL.Contexts;
using CurrentService.Infrastructure.DAL.Repositories;
using CurrencyService.Domain.Entities;
using CurrencyEntity = CurrencyService.Domain.Entities.Currency;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace CurrencyService.Tests.Repositories
{
    public class CurrencyRepositoryTests : IDisposable
    {
        private readonly CurrencyServiceContext _context;
        private readonly CurrencyRepository _repository;
        private readonly Mock<IMediator> _mediatorMock;

        public CurrencyRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<CurrencyServiceContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _mediatorMock = new Mock<IMediator>();
            _context = new CurrencyServiceContext(options, _mediatorMock.Object);
            _repository = new CurrencyRepository(_context);
        }

        [Fact]
        public async Task GetAllNames_WhenCurrenciesExist_ShouldReturnAllNames()
        {
            CurrencyEntity currency1 = new CurrencyEntity("USD", 75.50m);
            CurrencyEntity currency2 = new CurrencyEntity("EUR", 82.30m);
            CurrencyEntity currency3 = new CurrencyEntity("GBP", 95.20m);

            _context.Currencies.AddRange(currency1, currency2, currency3);
            await _context.SaveChangesAsync();

            CancellationToken cancellationToken = CancellationToken.None;

            (Guid id, string name)[] result = await _repository.GetAllNames(cancellationToken);

            Assert.Equal(3, result.Length);
            Assert.Contains(result, x => x.name == "USD");
            Assert.Contains(result, x => x.name == "EUR");
            Assert.Contains(result, x => x.name == "GBP");
        }

        [Fact]
        public async Task GetAllNames_WhenNoCurrencies_ShouldReturnEmptyArray()
        {
            CancellationToken cancellationToken = CancellationToken.None;

            (Guid id, string name)[] result = await _repository.GetAllNames(cancellationToken);

            Assert.Empty(result);
        }

        [Fact]
        public async Task GetAllNames_ShouldReturnCorrectIdAndName()
        {
            CurrencyEntity currency = new CurrencyEntity("USD", 75.50m);
            _context.Currencies.Add(currency);
            await _context.SaveChangesAsync();

            CancellationToken cancellationToken = CancellationToken.None;

            (Guid id, string name)[] result = await _repository.GetAllNames(cancellationToken);

            Assert.Single(result);
            Assert.Equal(currency.Id, result[0].id);
            Assert.Equal("USD", result[0].name);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}

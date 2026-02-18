using CurrencyService.Application.Currency.Queries.GetAllNames;
using CurrencyService.Application.Repositories;
using Moq;
using Xunit;

namespace CurrencyService.Tests.Currency
{
    public class GetAllNamesQueryHandlerTests
    {
        private readonly Mock<ICurrencyRepository> _mockRepository;
        private readonly GetAllNamesQueryHandler _handler;

        public GetAllNamesQueryHandlerTests()
        {
            _mockRepository = new Mock<ICurrencyRepository>();
            _handler = new GetAllNamesQueryHandler(_mockRepository.Object);
        }

        [Fact]
        public async Task Handle_WhenRepositoryReturnsData_ShouldReturnMappedNameItems()
        {
            var cancellationToken = CancellationToken.None;
            var query = new GetAllNamesQuery();
            
            var repositoryData = new[]
            {
                (id: Guid.NewGuid(), name: "USD"),
                (id: Guid.NewGuid(), name: "EUR"),
                (id: Guid.NewGuid(), name: "RUB")
            };

            _mockRepository
                .Setup(r => r.GetAllNames(cancellationToken))
                .ReturnsAsync(repositoryData);

            var result = await _handler.Handle(query, cancellationToken);

            Assert.NotNull(result);
            Assert.Equal(3, result.Length);
            Assert.Equal(repositoryData[0].id, result[0].Id);
            Assert.Equal(repositoryData[0].name, result[0].Name);
            Assert.Equal(repositoryData[1].id, result[1].Id);
            Assert.Equal(repositoryData[1].name, result[1].Name);
            Assert.Equal(repositoryData[2].id, result[2].Id);
            Assert.Equal(repositoryData[2].name, result[2].Name);

            _mockRepository.Verify(r => r.GetAllNames(cancellationToken), Times.Once);
        }

        [Fact]
        public async Task Handle_WhenRepositoryReturnsEmptyArray_ShouldReturnEmptyArray()
        {
            var cancellationToken = CancellationToken.None;
            var query = new GetAllNamesQuery();
            
            var emptyData = Array.Empty<(Guid id, string name)>();

            _mockRepository
                .Setup(r => r.GetAllNames(cancellationToken))
                .ReturnsAsync(emptyData);

 
            var result = await _handler.Handle(query, cancellationToken);

            Assert.NotNull(result);
            Assert.Empty(result);

        }

        [Fact]
        public async Task Handle_WhenRepositoryThrowsException_ShouldPropagateException()
        {
            var cancellationToken = CancellationToken.None;
            var query = new GetAllNamesQuery();
            var expectedException = new Exception("Repository error");

            _mockRepository
                .Setup(r => r.GetAllNames(cancellationToken))
                .ThrowsAsync(expectedException);

            await Assert.ThrowsAsync<Exception>(() => _handler.Handle(query, cancellationToken));
        }

        [Fact]
        public async Task Handle_WhenRepositoryReturnsSingleItem_ShouldReturnSingleNameItem()
        {
            var cancellationToken = CancellationToken.None;
            var query = new GetAllNamesQuery();
            var expectedId = Guid.NewGuid();
            var expectedName = "GBP";
            
            var repositoryData = new[] { (id: expectedId, name: expectedName) };

            _mockRepository
                .Setup(r => r.GetAllNames(cancellationToken))
                .ReturnsAsync(repositoryData);

            var result = await _handler.Handle(query, cancellationToken);

            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(expectedId, result[0].Id);
            Assert.Equal(expectedName, result[0].Name);
        }
    }
}

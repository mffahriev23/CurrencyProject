using Microsoft.EntityFrameworkCore;
using UserService.Domain.Entities;
using UserService.Infrastructure.DAL.Contexts;
using UserService.Infrastructure.DAL.Repositories;

namespace UserService.Tests.Repositories
{
    public class RefreshTokenRepositoryTests : IDisposable
    {
        private readonly UserServiceContext _context;
        private readonly RefreshTokenRepository _repository;

        public RefreshTokenRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<UserServiceContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new UserServiceContext(options);
            _repository = new RefreshTokenRepository(_context);
        }

        [Fact]
        public async Task Get_WhenValidActiveTokenExists_ShouldReturnToken()
        {
            Guid userId = Guid.NewGuid();
            string hash = "hash-1";

            RefreshToken valid = new()
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                HashToken = hash,
                Created = DateTime.UtcNow.AddDays(-1),
                Expires = DateTime.UtcNow.AddDays(1),
                IsRevoked = false
            };

            RefreshToken revoked = new()
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                HashToken = hash,
                Created = DateTime.UtcNow.AddDays(-2),
                Expires = DateTime.UtcNow.AddDays(1),
                IsRevoked = true
            };

            RefreshToken expired = new()
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                HashToken = hash,
                Created = DateTime.UtcNow.AddDays(-10),
                Expires = DateTime.UtcNow.AddDays(-1),
                IsRevoked = false
            };

            RefreshToken otherUser = new()
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                HashToken = hash,
                Created = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddDays(1),
                IsRevoked = false
            };

            _context.RefreshTokens.AddRange(valid, revoked, expired, otherUser);
            await _context.SaveChangesAsync();

            RefreshToken? result = await _repository.Get(userId, hash, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(valid.Id, result!.Id);
        }

        [Fact]
        public async Task Get_WhenNoMatchingToken_ShouldReturnNull()
        {
            Guid userId = Guid.NewGuid();
            string hash = "hash";

            RefreshToken token = new()
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                HashToken = hash,
                Created = DateTime.UtcNow.AddDays(-2),
                Expires = DateTime.UtcNow.AddDays(-1),
                IsRevoked = false
            };

            _context.RefreshTokens.Add(token);
            await _context.SaveChangesAsync();

            RefreshToken? result = await _repository.Get(userId, hash, CancellationToken.None);

            Assert.Null(result);
        }

        [Fact]
        public void Add_ShouldTrackRefreshTokenInContext()
        {
            RefreshToken token = new()
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                HashToken = "hash",
                Created = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddDays(1),
                IsRevoked = false
            };

            _repository.Add(token);

            Assert.Single(_context.RefreshTokens.Local);
            Assert.Equal(token.Id, _context.RefreshTokens.Local.Single().Id);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}


using Microsoft.EntityFrameworkCore;
using UserService.Domain.Entities;
using UserService.Infrastructure.DAL.Contexts;
using UserService.Infrastructure.DAL.Repositories;

namespace UserService.Tests.Repositories
{
    public class UserRepositoryTests : IDisposable
    {
        private readonly UserServiceContext _context;
        private readonly UserRepository _repository;

        public UserRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<UserServiceContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new UserServiceContext(options);
            _repository = new UserRepository(_context);
        }

        [Fact]
        public async Task GetUser_ByName_WhenUserExists_ShouldReturnUser()
        {
            User user = new()
            {
                Id = Guid.NewGuid(),
                Name = "test-user",
                Password = "pwd"
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            User? result = await _repository.GetUser("test-user", CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(user.Id, result!.Id);
            Assert.Equal("test-user", result.Name);
        }

        [Fact]
        public async Task GetUser_ByName_WhenUserDoesNotExist_ShouldReturnNull()
        {
            User? result = await _repository.GetUser("absent", CancellationToken.None);

            Assert.Null(result);
        }

        [Fact]
        public async Task GetUser_ById_WhenUserExists_ShouldReturnUser()
        {
            User user = new()
            {
                Id = Guid.NewGuid(),
                Name = "user-id",
                Password = "pwd"
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            User? result = await _repository.GetUser(user.Id, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(user.Id, result!.Id);
            Assert.Equal("user-id", result.Name);
        }

        [Fact]
        public async Task GetUser_ById_WhenUserDoesNotExist_ShouldReturnNull()
        {
            User? result = await _repository.GetUser(Guid.NewGuid(), CancellationToken.None);

            Assert.Null(result);
        }

        [Fact]
        public void Add_ShouldTrackUserInContext()
        {
            User user = new()
            {
                Id = Guid.NewGuid(),
                Name = "tracked-user",
                Password = "pwd"
            };

            _repository.Add(user);

            Assert.Single(_context.Users.Local);
            Assert.Equal(user.Id, _context.Users.Local.Single().Id);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}


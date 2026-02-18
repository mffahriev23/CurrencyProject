using Microsoft.EntityFrameworkCore;
using UserService.Application.Repositories;
using UserService.Domain.Entities;
using UserService.Infrastructure.DAL.Contexts;

namespace UserService.Infrastructure.DAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        readonly UserServiceContext _userServiceContext;

        public UserRepository(UserServiceContext userServiceContext)
        {
            _userServiceContext = userServiceContext;
        }

        public void Add(User user)
        {
            _userServiceContext.Users.Add(user);
        }

        public Task<User?> GetUser(string name, CancellationToken cancellationToken)
        {
            return _userServiceContext.Users.FirstOrDefaultAsync(
                x => x.Name == name,
                cancellationToken
            );
        }

        public Task<User?> GetUser(Guid id, CancellationToken cancellationToken)
        {
            return _userServiceContext.Users.FirstOrDefaultAsync(
                x => x.Id == id,
                cancellationToken
            );
        }
    }
}

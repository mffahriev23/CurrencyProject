using UserService.Domain.Entities;

namespace UserService.Application.Repositories
{
    public interface IUserRepository
    {
        void Add(User user);

        Task<User?> GetUser(string name, CancellationToken cancellationToken);

        Task<User?> GetUser(Guid id, CancellationToken cancellationToken);
    }
}

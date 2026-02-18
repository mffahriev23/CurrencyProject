using Microsoft.EntityFrameworkCore;
using UserService.Application.UnitOfWork;

namespace JobLoaderCurrency.Services
{
    public class UnitOfWork<TDbContext> : IUnitOfWork where TDbContext : DbContext
    {
        readonly TDbContext _dbContext;

        public UnitOfWork(TDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}

using Microsoft.EntityFrameworkCore;

namespace JobLoaderCurrency.Interfaces.UnitOfWork
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}

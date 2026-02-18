using CurrencyService.Domain.Entities;

namespace CurrencyService.Application.Repositories
{
    public interface IHistoryRepository
    {
        void Add(FavoriteHistory history);
    }
}

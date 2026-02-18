using CurrencyService.Application.Repositories;
using CurrencyService.Domain.Entities;
using CurrencyService.Domain.Events;
using MediatR;

namespace CurrencyService.Application.Favorites.Notifications
{
    public class DeletedFavoriteCurrencyHandler : INotificationHandler<DeletedFavoriteCurrencyEvent>
    {
        readonly IHistoryRepository _historyRepository;

        public DeletedFavoriteCurrencyHandler(IHistoryRepository historyRepository)
        {
            _historyRepository = historyRepository;
        }

        public async Task Handle(
            DeletedFavoriteCurrencyEvent request,
            CancellationToken cancellationToken
        )
        {
            FavoriteHistory history = new(
                request.Favorite.Id,
                Domain.Enums.FavoriteActions.Delete,
                request.Timestamp
            );

            _historyRepository.Add(history);
        }
    }
}

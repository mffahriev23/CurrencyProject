using CurrencyService.Application.Repositories;
using CurrencyService.Domain.Entities;
using CurrencyService.Domain.Events;
using MediatR;

namespace CurrencyService.Application.Favorites.Notifications
{
    public class AddedFavoriteCurrencyHandler : INotificationHandler<AddedFavoriteCurrencyEvent>
    {
        readonly IHistoryRepository _historyRepository;

        public AddedFavoriteCurrencyHandler(IHistoryRepository historyRepository)
        {
            _historyRepository = historyRepository;
        }

        public async Task Handle(AddedFavoriteCurrencyEvent request, CancellationToken cancellationToken)
        {
            FavoriteHistory history = new(
                request.Favorite.Id,
                Domain.Enums.FavoriteActions.Create,
                request.Timestamp
            );

            _historyRepository.Add(history);
        }
    }
}

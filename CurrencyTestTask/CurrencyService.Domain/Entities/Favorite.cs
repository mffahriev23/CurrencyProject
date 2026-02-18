using CurrencyService.Domain.Events;
using Domain;

namespace CurrencyService.Domain.Entities
{
    public class Favorite : DomainEventPublisherBase
    {
        private Guid _id;
        private Guid _userId;
        private Guid _currencyId;
        private bool _isActive;
        private Currency? _currency;
        private List<FavoriteHistory> _histories;

        public Guid UserId => _userId;

        public Guid CurrencyId => _currencyId;

        public Guid Id => _id;

        public bool IsActive => _isActive;

        public Currency? Currency => _currency;

        public List<FavoriteHistory> FavoriteEvents => _histories;

        private Favorite(
            Guid userId,
            Guid currencyId
        )
        {
            _userId = userId;
            _currencyId = currencyId;

            _histories = new List<FavoriteHistory>();
            _id = Guid.NewGuid();
            _isActive = true;
        }

        public static Favorite Create(
            DateTime timestamp,
            Guid userId,
            Guid currencyId
        )
        {
            Favorite favorite = new Favorite(
                userId,
                currencyId
            );

            AddedFavoriteCurrencyEvent @event = new(
                timestamp,
                favorite
            );

            favorite.AddEvent(@event);

            return favorite;
        }

        public void Delete(DateTime timestamp)
        {
            _isActive = false;

            DeletedFavoriteCurrencyEvent @event = new(
                timestamp,
                this
            );

            AddEvent(@event);
        }
    }
}

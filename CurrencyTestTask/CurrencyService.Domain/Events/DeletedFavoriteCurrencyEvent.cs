using CurrencyService.Domain.Entities;
using Domain;

namespace CurrencyService.Domain.Events
{
    public class DeletedFavoriteCurrencyEvent : BaseDomainEvent
    {
        private Favorite _favorite;

        public Favorite Favorite => _favorite;

        public DeletedFavoriteCurrencyEvent(
            DateTime timestamp,
            Favorite favorite
        ) : base(timestamp)
        {
            _favorite = favorite;
        }
    }
}

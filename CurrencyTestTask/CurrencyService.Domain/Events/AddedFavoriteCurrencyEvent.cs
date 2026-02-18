using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CurrencyService.Domain.Entities;
using Domain;

namespace CurrencyService.Domain.Events
{
    public class AddedFavoriteCurrencyEvent : BaseDomainEvent
    {
        private Favorite _favorite;

        public Favorite Favorite => _favorite;

        public AddedFavoriteCurrencyEvent(
            DateTime timestamp,
            Favorite favorite
        ) : base(timestamp)
        {
            _favorite = favorite;
        }
    }
}

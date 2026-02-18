using CurrencyService.Domain.Enums;

namespace CurrencyService.Domain.Entities
{
    public class FavoriteHistory
    {
        private Guid _id;
        private Guid _favoriteId;
        private FavoriteActions _action;
        private DateTime _created;
        private Favorite? _favorite;

        public Guid Id => _id;

        public Guid FavoriteId => _favoriteId;

        public FavoriteActions Action => _action;

        public DateTime Created => _created;

        public Favorite? Favorite => _favorite;

        public FavoriteHistory(
            Guid favoriteId,
            FavoriteActions action,
            DateTime created
        )
        {
            _id = Guid.NewGuid();
            _favoriteId = favoriteId;
            _action = action;
            _created = created;
        }

        public static FavoriteHistory CreateCreated(
            Guid favoriteId,
            DateTime created
        )
        {
            return new FavoriteHistory(
                favoriteId,
                FavoriteActions.Create,
                created
            );
        }

        public static FavoriteHistory CreateDeleted(
            Guid favoriteId,
            DateTime created
        )
        {
            return new FavoriteHistory(
                favoriteId,
                FavoriteActions.Delete,
                created
            );
        }
    }
}

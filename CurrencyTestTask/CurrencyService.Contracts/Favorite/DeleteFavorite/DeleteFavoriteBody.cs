namespace CurrencyService.Contracts.Favorite.DeleteFavorite
{
    public record DeleteFavoriteBody
    {
        public Guid[] CurrencyIds { get; init; }

        public DeleteFavoriteBody()
        {
            CurrencyIds = Array.Empty<Guid>();
        }
    }
}

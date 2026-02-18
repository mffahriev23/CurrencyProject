namespace CurrencyService.Contracts.Favorite.AddFavorite
{
    public record AddFavoriteBody
    {
        public Guid[] CurrencyIds { get; init; }

        public AddFavoriteBody()
        {
            CurrencyIds = Array.Empty<Guid>();
        }
    }
}

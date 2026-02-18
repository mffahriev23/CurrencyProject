namespace CurrencyService.Application.Favorites.Queries.GetFavorites
{
    public record FavoriteItem(
        Guid Id,
        Guid CurrencyId,
        string Name,
        decimal Rate
    );
}

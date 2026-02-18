using Microsoft.AspNetCore.Mvc;

namespace CurrencyService.Contracts.Favorite.AddFavorite
{
    public record AddFavoriteRequest
    {
        [FromBody]
        public AddFavoriteBody? Body { get; init; }
    }
}

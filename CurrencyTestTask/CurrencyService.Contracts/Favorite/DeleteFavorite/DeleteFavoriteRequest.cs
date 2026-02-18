using Microsoft.AspNetCore.Mvc;

namespace CurrencyService.Contracts.Favorite.DeleteFavorite
{
    public record DeleteFavoriteRequest
    {
        [FromBody]
        public DeleteFavoriteBody? Body { get; init; }
    }
}

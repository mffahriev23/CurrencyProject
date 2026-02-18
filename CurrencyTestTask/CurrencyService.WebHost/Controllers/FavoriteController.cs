using Authorization.Extensions;
using CurrencyService.Application.Favorites.Commands.AddFavorite;
using CurrencyService.Application.Favorites.Commands.RemoveFavorite;
using CurrencyService.Application.Favorites.Queries.GetFavorites;
using CurrencyService.Contracts.Favorite.AddFavorite;
using CurrencyService.Contracts.Favorite.DeleteFavorite;
using CurrencyService.Contracts.Favorite.GetFavorites;
using CurrencyService.WebHost.Mappings;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyService.WebHost.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FavoriteController : ControllerBase
    {
        readonly ISender _sender;

        public FavoriteController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost("add-favorite")]
        [Authorize]
        public async Task<IActionResult> AddFaforite(
            AddFavoriteRequest request,
            CancellationToken cancellationToken
        )
        {
            AddFavoriteCommand command = request.Map(User.Claims.GetUserId());

            await _sender.Send(command, cancellationToken);

            return Ok();
        }

        [HttpPost("delete-favorite")]
        [Authorize]
        public async Task<IActionResult> DeleteFaforite(
            DeleteFavoriteRequest request,
            CancellationToken cancellationToken
        )
        {
            RemoveFavoriteCommand command = request.Map(User.Claims.GetUserId());

            await _sender.Send(command, cancellationToken);

            return Ok();
        }

        [HttpGet("get-active-favorite")]
        [Authorize]
        public async Task<IActionResult> GetActiveFavorite(
            CancellationToken cancellationToken
        )
        {
            GetFavoritesQuery query = new(User.Claims.GetUserId());

            FavoriteItem[] result = await _sender.Send(query, cancellationToken);

            FavoritesResponse response = result.Map();

            return Ok(response);
        }
    }
}

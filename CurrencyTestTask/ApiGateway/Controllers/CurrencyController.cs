using ApiGateway.Interfaces.UserService;
using CurrencyService.Contracts.Currency.GetAllNames;
using CurrencyService.Contracts.Favorite.AddFavorite;
using CurrencyService.Contracts.Favorite.DeleteFavorite;
using CurrencyService.Contracts.Favorite.GetFavorites;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiGateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyController : ControllerBase
    {
        readonly ICurrencyServiceClient _currencyServiceClient;

        public CurrencyController(ICurrencyServiceClient currencyServiceClient)
        {
            _currencyServiceClient = currencyServiceClient;
        }

        [HttpGet("get-all")]
        [Authorize]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            string? authorization = Request.Headers["Authorization"];

            GetAllNamesResponse response = await _currencyServiceClient.GetAll(
                authorization!,
                cancellationToken
            );

            return Ok(response);
        }

        [HttpGet("get-active-favorite")]
        [Authorize]
        public async Task<IActionResult> GetActiveFavorite(CancellationToken cancellationToken)
        {
            string? authorization = Request.Headers["Authorization"];

            FavoritesResponse response = await _currencyServiceClient.GetActiveFavorite(
                authorization!,
                cancellationToken
            );

            return Ok(response);
        }

        [HttpPost("delete-favorite")]
        [Authorize]
        public async Task<IActionResult> DeleteFaforite(
            DeleteFavoriteRequest request,
            CancellationToken cancellationToken
        )
        {
            string? authorization = Request.Headers["Authorization"];

            await _currencyServiceClient.DeleteFavorite(
                authorization!,
                request,
                cancellationToken
            );

            return Ok();
        }

        [HttpPost("add-favorite")]
        [Authorize]
        public async Task<IActionResult> AddFaforite(
            AddFavoriteRequest request,
            CancellationToken cancellationToken
        )
        {
            string? authorization = Request.Headers["Authorization"];

            await _currencyServiceClient.AddFavoriteRequest(
                authorization!,
                request,
                cancellationToken
            );

            return Ok();
        }
    }
}

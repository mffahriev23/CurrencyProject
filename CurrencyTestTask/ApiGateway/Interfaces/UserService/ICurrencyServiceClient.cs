using CurrencyService.Contracts.Currency.GetAllNames;
using CurrencyService.Contracts.Favorite.AddFavorite;
using CurrencyService.Contracts.Favorite.DeleteFavorite;
using CurrencyService.Contracts.Favorite.GetFavorites;

namespace ApiGateway.Interfaces.UserService
{
    public interface ICurrencyServiceClient
    {
        Task<GetAllNamesResponse> GetAll(string accessToken, CancellationToken cancellationToken);

        Task<FavoritesResponse> GetActiveFavorite(string accessToken, CancellationToken cancellationToken);

        Task DeleteFavorite(string accessToken, DeleteFavoriteRequest request, CancellationToken cancellationToken);

        Task AddFavoriteRequest (string accessToken, AddFavoriteRequest request, CancellationToken cancellationToken);
    }
}

using ApiGateway.Interfaces.UserService;
using CurrencyService.Contracts.Currency.GetAllNames;
using CurrencyService.Contracts.Favorite.AddFavorite;
using CurrencyService.Contracts.Favorite.DeleteFavorite;
using CurrencyService.Contracts.Favorite.GetFavorites;

namespace ApiGateway.Clients
{
    public class CurrencyServiceClient : ICurrencyServiceClient
    {
        readonly HttpClient _client;

        const string _getAllEndpoint = "/api/Currency/get-all";
        const string _addFavorite = "/api/Favorite/add-favorite";
        const string _deleteFavorite = "/api/Favorite/delete-favorite";
        const string _getActiveFavorite = "/api/Favorite/get-active-favorite";

        public CurrencyServiceClient(HttpClient httpClient)
        {
            _client = httpClient;
        }

        public async Task AddFavoriteRequest(
            string accessToken,
            AddFavoriteRequest request,
            CancellationToken cancellationToken
        )
        {
            _client.DefaultRequestHeaders.Add("Authorization", accessToken);

            using HttpResponseMessage message = await _client.PostAsJsonAsync(
                _addFavorite,
                request.Body,
                cancellationToken
            );
        }

        public async Task DeleteFavorite(
            string accessToken,
            DeleteFavoriteRequest request,
            CancellationToken cancellationToken
        )
        {
            _client.DefaultRequestHeaders.Add("Authorization", accessToken);

            using HttpResponseMessage message = await _client.PostAsJsonAsync(
                _deleteFavorite,
                request.Body,
                cancellationToken
            );
        }

        public async Task<FavoritesResponse?> GetActiveFavorite(
            string accessToken,
            CancellationToken cancellationToken
        )
        {
            _client.DefaultRequestHeaders.Add("Authorization", accessToken);

            using HttpResponseMessage response = await _client.GetAsync(_getActiveFavorite, cancellationToken);

            return await response.Content.ReadFromJsonAsync<FavoritesResponse>(cancellationToken);
        }

        public async Task<GetAllNamesResponse?> GetAll(
            string accessToken,
            CancellationToken cancellationToken
        )
        {
            _client.DefaultRequestHeaders.Add("Authorization", accessToken);

            using HttpResponseMessage response = await _client.GetAsync(_getAllEndpoint, cancellationToken);

            return await response.Content.ReadFromJsonAsync<GetAllNamesResponse>(cancellationToken);
        }
    }
}

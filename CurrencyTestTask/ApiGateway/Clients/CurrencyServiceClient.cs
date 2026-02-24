using ApiGateway.Interfaces.UserService;
using Authorization.Exceptions;
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

            if (!message.IsSuccessStatusCode)
            {
                await HandleError(message, cancellationToken);
            }
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

            if (!message.IsSuccessStatusCode)
            {
                await HandleError(message, cancellationToken);
            }
        }

        public async Task<FavoritesResponse?> GetActiveFavorite(
            string accessToken,
            CancellationToken cancellationToken
        )
        {
            _client.DefaultRequestHeaders.Add("Authorization", accessToken);

            using HttpResponseMessage response = await _client.GetAsync(_getActiveFavorite, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                await HandleError(response, cancellationToken);
            }

            return await response.Content.ReadFromJsonAsync<FavoritesResponse>(cancellationToken);
        }

        public async Task<GetAllNamesResponse?> GetAll(
            string accessToken,
            CancellationToken cancellationToken
        )
        {
            _client.DefaultRequestHeaders.Add("Authorization", accessToken);

            using HttpResponseMessage response = await _client.GetAsync(_getAllEndpoint, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                await HandleError(response, cancellationToken);
            }

            return await response.Content.ReadFromJsonAsync<GetAllNamesResponse>(cancellationToken);
        }

        private Task HandleError(HttpResponseMessage message, CancellationToken cancellationToken)
        {
            switch (message.StatusCode)
            {
                case System.Net.HttpStatusCode.BadRequest:
                {
                    return Handle400Error(message, cancellationToken);
                }
                default:
                {
                    throw new InternalServerErrorException("Неизвестная внутренняя ошибка сервиса.");
                }
            }
        }

        private async Task Handle400Error(HttpResponseMessage message, CancellationToken cancellationToken)
        {
            string errorContent = await message.Content.ReadAsStringAsync();

            throw new ExternalServiceReturnedBadRequestException(errorContent);
        }
    }
}

using System.Text.Json;
using ApiGateway.Interfaces.UserService;
using Authorization.Exceptions;
using UserService.Contracts.Users.Authentication;
using UserService.Contracts.Users.Logout;
using UserService.Contracts.Users.Refresh;
using UserService.Contracts.Users.Registration;

namespace ApiGateway.Clients
{
    public class UserServiceClient : IUserServiceClient
    {
        readonly HttpClient _client;
        const string _registrationEndpoint = "/api/User/registration";
        const string _authenticationEndpoint = "/api/User/authentication";
        const string _logoutEndpoint = "/api/User/logout";
        const string _refreshEndpoint = "/api/User/refresh";

        public UserServiceClient(HttpClient client)
        {
            _client = client;
        }

        public async Task<AuthenticationResponse?> Authentication(
            AuthenticationRequest request,
            CancellationToken cancellationToken
        )
        {
            using HttpResponseMessage message = await _client.PostAsJsonAsync(
                _authenticationEndpoint,
                request.Body,
                cancellationToken
            );

            if (!message.IsSuccessStatusCode)
            {
                await HandleError(message, cancellationToken);
            }

            return await message.Content.ReadFromJsonAsync<AuthenticationResponse>(cancellationToken);
        }

        public async Task LogOut(LogOutRequest request, string accessToken, CancellationToken cancellationToken)
        {
            _client.DefaultRequestHeaders.Add("Authorization", accessToken);

            using HttpResponseMessage message = await _client.PostAsJsonAsync(
                _logoutEndpoint,
                request.Body,
                cancellationToken
            );

            if (!message.IsSuccessStatusCode)
            {
                await HandleError(message, cancellationToken);
            }
        }

        public async Task<RefreshTokenResponse?> RefreshToken(
            RefreshTokenRequest request,
            string accessToken,
            CancellationToken cancellationToken
        )
        {
            _client.DefaultRequestHeaders.Add("Authorization", accessToken);

            using HttpResponseMessage message = await _client.PostAsJsonAsync(
                _refreshEndpoint,
                request.Body,
                cancellationToken
            );

            if (!message.IsSuccessStatusCode)
            {
                await HandleError(message, cancellationToken);
            }

            return await message.Content.ReadFromJsonAsync<RefreshTokenResponse>(cancellationToken);
        }

        public async Task Registration(RegistrationRequest request, CancellationToken cancellationToken)
        {
            using HttpResponseMessage message = await _client.PostAsJsonAsync(
                _registrationEndpoint,
                request.Body,
                cancellationToken
            );
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

using ApiGateway.Interfaces.UserService;
using UserService.Contracts.Users.Authentication;
using UserService.Contracts.Users.Logout;
using UserService.Contracts.Users.Refresh;
using UserService.Contracts.Users.Registration;

namespace ApiGateway.Clients
{
    public class UserServiceClient : BaseServiceClient, IUserServiceClient
    {
        const string _registrationEndpoint = "/api/User/registration";
        const string _authenticationEndpoint = "/api/User/authentication";
        const string _logoutEndpoint = "/api/User/logout";
        const string _refreshEndpoint = "/api/User/refresh";

        public UserServiceClient(HttpClient httpClient)
            : base(httpClient)
        {
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
    }
}

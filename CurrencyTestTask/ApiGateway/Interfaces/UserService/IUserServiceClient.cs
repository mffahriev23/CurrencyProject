using UserService.Contracts.Users.Authentication;
using UserService.Contracts.Users.Logout;
using UserService.Contracts.Users.Refresh;
using UserService.Contracts.Users.Registration;

namespace ApiGateway.Interfaces.UserService
{
    public interface IUserServiceClient
    {
        Task Registration(
            RegistrationRequest request,
            CancellationToken cancellationToken
        );

        Task<AuthenticationResponse> Authentication(
            AuthenticationRequest request,
            CancellationToken cancellationToken
        );

        Task LogOut(
            LogOutRequest request,
            string accessToken,
            CancellationToken cancellationToken
        );

        Task<RefreshTokenResponse> RefreshToken(
           RefreshTokenRequest request,
           string accessToken,
           CancellationToken cancellationToken
        );
    }
}

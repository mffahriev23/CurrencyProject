namespace UserService.Application.Users.Commands.Authentication
{
    public record AuthenticationResult(string AccessToken, string RefreshToken);
}

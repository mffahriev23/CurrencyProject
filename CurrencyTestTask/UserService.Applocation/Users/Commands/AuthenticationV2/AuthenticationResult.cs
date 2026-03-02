namespace UserService.Application.Users.Commands.AuthenticationV2
{
    public record AuthenticationResult(string AccessToken, string RefreshToken);
}

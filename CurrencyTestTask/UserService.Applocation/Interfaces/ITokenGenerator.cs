namespace UserService.Application.Interfaces
{
    public interface ITokenGenerator
    {
        string GenerateRefreshToken();

        string GenerateAccessToken(Guid userId, string username);
    }
}

using System.Security.Claims;

namespace Authorization.Interfaces
{
    public interface IJwtFactory
    {
        string GetJwtToken(Guid userId, string name);

        string GetJwtToken(Guid key);
    }
}

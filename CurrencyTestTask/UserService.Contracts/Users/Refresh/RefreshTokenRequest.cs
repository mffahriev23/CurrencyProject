using Microsoft.AspNetCore.Mvc;

namespace UserService.Contracts.Users.Refresh
{
    public record RefreshTokenRequest
    {
        [FromBody]
        public RefreshTokenBody? Body { get; init; }
    }
}

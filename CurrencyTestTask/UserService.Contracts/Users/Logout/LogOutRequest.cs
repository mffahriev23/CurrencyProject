using Microsoft.AspNetCore.Mvc;

namespace UserService.Contracts.Users.Logout
{
    public record LogOutRequest
    {
        [FromBody]
        public LogOutBody? Body { get; init; }
    }
}

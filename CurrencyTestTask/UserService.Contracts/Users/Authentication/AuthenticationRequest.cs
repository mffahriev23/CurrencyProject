using Microsoft.AspNetCore.Mvc;

namespace UserService.Contracts.Users.Authentication
{
    public record AuthenticationRequest
    {
        [FromBody]
        public AuthenticationBody? Body { get; init; }
    }
}

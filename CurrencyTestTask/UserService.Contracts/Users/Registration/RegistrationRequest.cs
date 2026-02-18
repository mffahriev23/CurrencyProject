using Microsoft.AspNetCore.Mvc;

namespace UserService.Contracts.Users.Registration
{
    public record RegistrationRequest
    {
        [FromBody]
        public RegistrationBody? Body { get; init; }
    }
}

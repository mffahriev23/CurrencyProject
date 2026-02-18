using MediatR;

namespace UserService.Application.Users.Commands.Authentication
{
    public record AuthenticationCommand(string Name, string Password) : IRequest<AuthenticationResult>;
}

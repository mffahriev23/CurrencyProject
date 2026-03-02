using MediatR;

namespace UserService.Application.Users.Commands.AuthenticationV2
{
    public record AuthenticationCommand(string Name, string Password) : IRequest<AuthenticationResult>;
}

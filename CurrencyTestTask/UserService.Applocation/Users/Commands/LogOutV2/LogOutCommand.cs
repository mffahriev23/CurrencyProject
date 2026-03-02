using MediatR;

namespace UserService.Application.Users.Commands.LogOutV2
{
    public record LogOutCommand(Guid UserId, string RefreshToken) : IRequest;
}

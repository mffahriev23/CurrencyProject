using MediatR;

namespace UserService.Application.Users.Commands.LogOut
{
    public record LogOutCommand(Guid UserId, string RefreshToken) : IRequest;
}

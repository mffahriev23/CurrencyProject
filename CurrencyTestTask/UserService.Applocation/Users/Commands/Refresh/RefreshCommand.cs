using MediatR;

namespace UserService.Application.Users.Commands.Refresh
{
    public record RefreshCommand(Guid UserId, string RefreshToken) : IRequest<RefreshResult>;
}

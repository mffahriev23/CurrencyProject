using MediatR;

namespace UserService.Application.Users.Commands.RefreshV2
{
    public record RefreshCommand(string AccessToken, string RefreshToken) : IRequest<RefreshResult>;
}

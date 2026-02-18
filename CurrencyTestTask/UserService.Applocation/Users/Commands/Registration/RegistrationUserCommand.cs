using MediatR;

namespace UserService.Application.Users.Commands.Registration
{
    public record RegistrationUserCommand(
        string Name,
        string Password,
        string DoublePassword
    ) : IRequest;
}

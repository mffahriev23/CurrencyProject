using UserService.Application.Users.Commands.Authentication;
using UserService.Application.Users.Commands.Registration;
using UserService.Contracts.Users.Authentication;
using UserService.Contracts.Users.Registration;

namespace UserService.WebHost.Mappings
{
    public static class UserMapping
    {
        public static RegistrationUserCommand Map(this RegistrationRequest request)
        {
            return new(
                request.Body!.Name!,
                request.Body!.Password!,
                request.Body!.DoublePassword!
            );
        }

        public static AuthenticationCommand Map(this AuthenticationRequest request)
        {
            return new(request.Body!.Name!, request.Body!.Password!);
        }
    }
}

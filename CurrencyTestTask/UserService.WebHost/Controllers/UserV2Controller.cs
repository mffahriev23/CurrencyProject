using Application.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserService.Application.Users.Commands.AuthenticationV2;
using UserService.Application.Users.Commands.LogOutV2;
using UserService.Application.Users.Commands.RefreshV2;
using UserService.Application.Users.Commands.Registration;
using UserService.Contracts.Users.Authentication;
using UserService.Contracts.Users.Logout;
using UserService.Contracts.Users.Refresh;
using UserService.Contracts.Users.Registration;
using UserService.WebHost.Mappings;

namespace UserService.WebHost.Controllers
{
    [Route("api/v2/[controller]")]
    [ApiController]
    public class UserV2Controller : ControllerBase
    {
        readonly ISender _sender;

        public UserV2Controller(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost("registration")]
        [AllowAnonymous]
        public async Task<IActionResult> Registration(
            RegistrationRequest request,
            CancellationToken cancellationToken
        )
        {
            RegistrationUserCommand command = request.Map();

            await _sender.Send(command, cancellationToken);

            return Ok();
        }

        [HttpPost("authentication")]
        public async Task<AuthenticationResponse> Authentication(
            AuthenticationRequest request,
            CancellationToken cancellationToken
        )
        {
            AuthenticationCommand command = new(
                request.Body!.Name!,
                request.Body.Password!
            );

            AuthenticationResult result = await _sender.Send(
                command,
                cancellationToken
            );

            AuthenticationResponse response = new()
            {
                AccessToken = result.AccessToken,
                RefreshToken = result.RefreshToken
            };

            return response;
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> LogOut(
            LogOutRequest request,
            CancellationToken cancellationToken
        )
        {
            LogOutCommand command = new(
                User.Claims.GetUserId(),
                request.Body!.RefreshToken!
            );

            await _sender.Send(command, cancellationToken);

            return Ok();
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh(
            RefreshTokenRequest request,
            CancellationToken cancellationToken
        )
        {
            RefreshCommand command = new(
                request.Body!.AccessToken!,
                request.Body!.RefreshToken!
            );

            RefreshResult result = await _sender.Send(
                command,
                cancellationToken
            );

            RefreshTokenResponse response = new()
            {
                AccessToken = result.AccessToken,
                RefreshToken = result.RefreshToken
            };

            return Ok(response);
        }
    }
}

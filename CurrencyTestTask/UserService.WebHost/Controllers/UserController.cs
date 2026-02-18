using Authorization.Attributes;
using Authorization.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserService.Application.Users.Commands.Authentication;
using UserService.Application.Users.Commands.LogOut;
using UserService.Application.Users.Commands.Refresh;
using UserService.Application.Users.Commands.Registration;
using UserService.Contracts.Users.Authentication;
using UserService.Contracts.Users.Logout;
using UserService.Contracts.Users.Refresh;
using UserService.Contracts.Users.Registration;
using UserService.WebHost.Mappings;

namespace UserService.WebHost.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        readonly ISender _sender;

        public UserController(ISender sender)
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
        [AllowAnonymous]
        public async Task<IActionResult> Authentication(
            AuthenticationRequest request,
            CancellationToken cancellationToken
        )
        {
            AuthenticationCommand command = request.Map();

            AuthenticationResult result = await _sender.Send(command, cancellationToken);

            AuthenticationResponse response = new()
            {
                AccessToken = result.AccessToken,
                RefreshToken = result.RefreshToken
            };

            return Ok(response);
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
        [Authorize]
        [AccessExpiredToken]
        public async Task<IActionResult> Refresh(
            RefreshTokenRequest request,
            CancellationToken cancellationToken
        )
        {
            RefreshCommand command = new(
                User.Claims.GetUserId(),
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

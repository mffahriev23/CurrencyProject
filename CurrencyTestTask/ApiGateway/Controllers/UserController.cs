using ApiGateway.Interfaces.UserService;
using Authorization.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using UserService.Contracts.Users.Authentication;
using UserService.Contracts.Users.Logout;
using UserService.Contracts.Users.Refresh;
using UserService.Contracts.Users.Registration;

namespace ApiGateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        readonly IUserServiceClient _userServiceClient;
        readonly static CookieOptions _cookieOptions = new()
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.UtcNow.AddDays(7)
        };

        public UserController(IUserServiceClient userServiceClient)
        {
            _userServiceClient = userServiceClient;
        }

        [HttpPost("registration")]
        [AllowAnonymous]
        public async Task<IActionResult> Registration(RegistrationRequest request, CancellationToken cancellationToken)
        {
             await _userServiceClient.Registration(
                 request,
                 cancellationToken
             );

            return Ok();
        }

        [HttpPost("authentication")]
        [AllowAnonymous]
        public async Task<IActionResult> Authentication(AuthenticationRequest request, CancellationToken cancellationToken)
        {
            AuthenticationResponse response = await _userServiceClient.Authentication(
                request,
                cancellationToken
            );

            Response.Cookies.Append("refreshToken", response.RefreshToken, _cookieOptions);

            return Ok(response);
        }

        [HttpPost("refresh")]
        [Authorize]
        [AccessExpiredToken]
        public async Task<IActionResult> Refresh(CancellationToken cancellationToken)
        {
            RefreshTokenRequest request = new()
            {
                Body = new RefreshTokenBody
                {
                    RefreshToken = Request.Cookies["refreshToken"]
                }
            };

            string authorization = Request.Headers["Authorization"];

            RefreshTokenResponse response = await _userServiceClient.RefreshToken(
                request,
                authorization,
                cancellationToken
            );

            Response.Cookies.Append("refreshToken", response.RefreshToken, _cookieOptions);

            return Ok(response);
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout(CancellationToken cancellationToken)
        {
            LogOutRequest request = new()
            {
                Body = new LogOutBody
                {
                    RefreshToken = Request.Cookies["refreshToken"]
                }
            };

            string authorization = Request.Headers["Authorization"];

            await _userServiceClient.LogOut(
                request,
                authorization,
                cancellationToken
            );

            Response.Cookies.Delete("refreshToken", _cookieOptions);

            return Ok();
        }
    }
}

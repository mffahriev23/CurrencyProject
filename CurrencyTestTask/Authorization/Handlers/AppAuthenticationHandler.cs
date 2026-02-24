using System.Security.Claims;
using System.Text.Encodings.Web;
using Authorization.Attributes;
using Authorization.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace Authorization.Handlers
{
    internal class AppAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly IJwtReader _jwtManager;
        const string _headerName = "Authorization";

        public AppAuthenticationHandler(
                UrlEncoder encoder,
                ILoggerFactory loggerFactory,
                IJwtReader jwtManager,
                IOptionsMonitor<AuthenticationSchemeOptions> optionMonitor
            ) : base(optionMonitor, loggerFactory, encoder)
        {
            _jwtManager = jwtManager;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (Context.GetEndpoint()?.Metadata?.GetMetadata<IAllowAnonymous>() is null)
            {
                bool validateExpirationTime = Context.GetEndpoint()?.Metadata.GetMetadata<AccessExpiredTokenAttribute>()
                    is null;

                if (!Request.Headers.ContainsKey(_headerName))
                {
                    return AuthenticateResult.Fail("Пользователь не аутентифицирован.");
                }

                Microsoft.Extensions.Primitives.StringValues headerAuth = Request.Headers[_headerName];
                string? jwtText = headerAuth[0];

                if (string.IsNullOrEmpty(jwtText))
                {
                    throw new ArgumentException("Authorization не был найден");
                }

                try
                {
                    Claim[] claims = _jwtManager.GetClaims(jwtText, validateExpirationTime);
                    ClaimsIdentity identity = new(claims, Scheme.Name);
                    ClaimsPrincipal principal = new(identity);
                    AuthenticationTicket ticket = new(principal, Scheme.Name);
                    return AuthenticateResult.Success(ticket);
                }
                catch
                {
                    return AuthenticateResult.Fail("Пользователь не аутентифицирован.");
                }
            }

            return AuthenticateResult.NoResult();
        }
    }
}
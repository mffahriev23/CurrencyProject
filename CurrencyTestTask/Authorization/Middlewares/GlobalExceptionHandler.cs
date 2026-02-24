using Authorization.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Authorization.Middlewares
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken
        )
        {
            switch (exception)
            {
                case ExternalServiceReturnedBadRequestException:
                {
                    await ExternalServiceReturnedBadRequestError(
                        httpContext,
                        exception,
                        cancellationToken
                    );

                    break;
                }

                case BadRequestException:
                {
                    await BadRequestError(
                        httpContext,
                        exception,
                        cancellationToken
                    );

                    break;
                }

                default:
                {
                    await InternalError(
                        httpContext,
                        exception,
                        cancellationToken
                    );

                    break;
                }
            }

            return true;
        }

        private async Task ExternalServiceReturnedBadRequestError(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken
        )
        {
            ProblemDetails problemDetails = new()
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Внешний сервис вернул 400.",
                Detail = exception.Message
            };

            httpContext.Response.StatusCode = problemDetails.Status.Value;

            await httpContext.Response.WriteAsJsonAsync(
                problemDetails,
                cancellationToken
            );
        }

        private async Task BadRequestError(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken
        )
        {
            ProblemDetails problemDetails = new()
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Ошибка валидации.",
                Detail = exception.Message
            };

            httpContext.Response.StatusCode = problemDetails.Status.Value;

            await httpContext.Response.WriteAsJsonAsync(
                problemDetails,
                cancellationToken
            );
        }

        private async Task InternalError(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken
        )
        {
            _logger.LogError(
                        exception,
                        "Произошло необработанное исключение: {Message}",
                        exception.Message
                    );

            ProblemDetails problemDetails = new()
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "Ошибка сервера",
                Detail = "Внутренняя ошибка. Пожалуйста, попробуйте позже."
            };

            httpContext.Response.StatusCode = problemDetails.Status.Value;

            await httpContext.Response.WriteAsJsonAsync(
                problemDetails,
                cancellationToken
            );
        }
    }
}

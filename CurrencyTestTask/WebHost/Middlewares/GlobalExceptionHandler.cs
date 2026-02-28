using Application.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace WebHost.Middlewares
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
                case ExternalServiceReturnedForbiddenException:
                {
                    await ExternalServiceReturnedForbiddenErrorHandle(
                        httpContext,
                        exception,
                        cancellationToken
                    );

                    break;
                }

                case ExternalServiceReturnedBadRequestException:
                {
                    await ExternalServiceReturnedBadRequestErrorHandle(
                        httpContext,
                        exception,
                        cancellationToken
                    );

                    break;
                }

                case BadRequestException:
                {
                    await BadRequestErrorHandle(
                        httpContext,
                        exception,
                        cancellationToken
                    );

                    break;
                }

                case ForbiddenException:
                {
                    await ForbiddenErrorHandle(
                        httpContext,
                        exception,
                        cancellationToken
                    );

                    break;
                }

                default:
                {
                    await InternalErrorHandle(
                        httpContext,
                        exception,
                        cancellationToken
                    );

                    break;
                }
            }

            return true;
        }

        private async Task ExternalServiceReturnedForbiddenErrorHandle(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken
        )
        {
            ProblemDetails problemDetails = new()
            {
                Status = StatusCodes.Status403Forbidden,
                Title = $"Внешний сервис вернул {StatusCodes.Status403Forbidden}.",
                Detail = exception.Message
            };

            httpContext.Response.StatusCode = problemDetails.Status.Value;

            await httpContext.Response.WriteAsJsonAsync(
                problemDetails,
                cancellationToken
            );
        }

        private async Task ForbiddenErrorHandle(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken
        )
        {
            ProblemDetails problemDetails = new()
            {
                Status = StatusCodes.Status403Forbidden,
                Title = "Ошибка авторизации.",
                Detail = exception.Message
            };

            httpContext.Response.StatusCode = problemDetails.Status.Value;

            await httpContext.Response.WriteAsJsonAsync(
                problemDetails,
                cancellationToken
            );
        }

        private async Task ExternalServiceReturnedBadRequestErrorHandle(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken
        )
        {
            ProblemDetails problemDetails = new()
            {
                Status = StatusCodes.Status400BadRequest,
                Title = $"Внешний сервис вернул {StatusCodes.Status400BadRequest}.",
                Detail = exception.Message
            };

            httpContext.Response.StatusCode = problemDetails.Status.Value;

            await httpContext.Response.WriteAsJsonAsync(
                problemDetails,
                cancellationToken
            );
        }

        private async Task BadRequestErrorHandle(
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

        private async Task InternalErrorHandle(
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

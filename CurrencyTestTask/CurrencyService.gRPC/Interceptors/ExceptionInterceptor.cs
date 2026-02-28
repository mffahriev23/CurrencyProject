using Application.Exceptions;
using Grpc.Core;
using Grpc.Core.Interceptors;

namespace CurrencyService.gRPC.Interceptors
{
    public class ExceptionInterceptor : Interceptor
    {
        readonly ILogger<ExceptionInterceptor> _logger;

        public ExceptionInterceptor(ILogger<ExceptionInterceptor> logger)
        {
            _logger = logger;
        }

        public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
            TRequest request,
             ServerCallContext context,
             UnaryServerMethod<TRequest, TResponse> continuation
        )
        {
            try
            {
                return await continuation(request, context);
        
            }
            catch (Exception ex)
            {
                Status status = ex switch
                {
                    BadRequestException => new Status(StatusCode.InvalidArgument, ex.Message),
                    ForbiddenException => new Status(StatusCode.Unauthenticated, ex.Message),
                    _ => new Status(StatusCode.Internal, "Внутренняя ошибка сервера")
                };

                if (status.StatusCode == StatusCode.Internal)
                {
                    _logger.LogError(
                        ex,
                        "Ошибка при выполнении gRPC-вызова: {Message}",
                        ex.Message
                    );
                }

                context.Status = status;

                throw new RpcException(status);

            }
        }
    }
}

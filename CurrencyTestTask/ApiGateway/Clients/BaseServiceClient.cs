using Application.Exceptions;

namespace ApiGateway.Clients
{
    public abstract class BaseServiceClient
    {
        protected readonly HttpClient _client;

        protected BaseServiceClient(HttpClient httpClient)
        {
            _client = httpClient;
        }

        protected Task HandleError(HttpResponseMessage message, CancellationToken cancellationToken)
        {
            switch (message.StatusCode)
            {
                case System.Net.HttpStatusCode.BadRequest:
                {
                    return Handle400Error(message, cancellationToken);
                }
                default:
                {
                    throw new InternalServerErrorException("Неизвестная внутренняя ошибка сервиса.");
                }
            }
        }

        protected async Task Handle400Error(HttpResponseMessage message, CancellationToken cancellationToken)
        {
            string errorContent = await message.Content.ReadAsStringAsync();

            throw new ExternalServiceReturnedBadRequestException(errorContent);
        }
    }
}

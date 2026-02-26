namespace Application.Exceptions
{
    public class ExternalServiceReturnedBadRequestException : Exception
    {
        public ExternalServiceReturnedBadRequestException(string message)
            : base(message)
        {
        }
    }
}

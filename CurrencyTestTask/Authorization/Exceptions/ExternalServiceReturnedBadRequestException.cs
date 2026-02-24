namespace Authorization.Exceptions
{
    public class ExternalServiceReturnedBadRequestException : Exception
    {
        public ExternalServiceReturnedBadRequestException(string message)
            : base(message)
        {
        }
    }
}

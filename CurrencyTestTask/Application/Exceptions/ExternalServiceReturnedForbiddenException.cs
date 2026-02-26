namespace Application.Exceptions
{
    public class ExternalServiceReturnedForbiddenException : Exception
    {
        public ExternalServiceReturnedForbiddenException(string message)
            :base(message)
        {
        }
    }
}

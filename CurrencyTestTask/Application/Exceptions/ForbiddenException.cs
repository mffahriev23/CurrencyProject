namespace Application.Exceptions
{
    public class ForbiddenException : Exception
    {
        public ForbiddenException(string message)
            : base(message)
        {
        }

        public ForbiddenException(string message, Exception exception)
            : base(message, exception)
        {
        }
    }
}

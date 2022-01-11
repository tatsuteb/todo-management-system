namespace UseCase.Shared
{
    public class UseCaseException : Exception
    {
        public int? StatusCode { get; }

        public UseCaseException(string? message, int? statusCode = null) : base(message)
        {
            StatusCode = statusCode;
        }
    }
}
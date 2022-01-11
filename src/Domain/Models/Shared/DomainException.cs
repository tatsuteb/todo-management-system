namespace Domain.Models.Shared
{
    public class DomainException : Exception
    {
        public int? StatusCode { get; }

        public DomainException(string? message, int? statusCode = null) : base(message)
        {
            StatusCode = statusCode;
        }
    }
}
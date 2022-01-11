namespace WebClient.Models.Shared.Exceptions
{
    public class WebClientExceptionResponseModel
    {
        public string Message { get; }
        public object? Data { get; }

        public WebClientExceptionResponseModel(string message, object? data = null)
        {
            Message = message;
            Data = data;
        }
    }
}

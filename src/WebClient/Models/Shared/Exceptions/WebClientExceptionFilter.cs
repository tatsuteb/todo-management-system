using Domain.Models.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using UseCase.Shared;

namespace WebClient.Models.Shared.Exceptions
{
    public class WebClientExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<WebClientExceptionFilter> _logger;

        public WebClientExceptionFilter(ILogger<WebClientExceptionFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            // 想定外のエラー（デフォルト）
            var value = new WebClientExceptionResponseModel(
                message: "不明なエラーが発生しました");
            var statusCode = StatusCodes.Status500InternalServerError;

            switch (context.Exception)
            {
                case null:
                    return;

                // 想定内のエラー
                case DomainException domainException:
                {
                    value = new WebClientExceptionResponseModel(
                        message: domainException.Message);
                    statusCode = domainException?.StatusCode ?? StatusCodes.Status400BadRequest;
                    break;
                }
                case UseCaseException useCaseException:
                {
                    value = new WebClientExceptionResponseModel(
                        message: useCaseException.Message);
                    statusCode = useCaseException?.StatusCode ?? StatusCodes.Status400BadRequest;
                    break;
                }
                default:
                {
                    _logger.LogError(context.Exception.Message);
                    break;
                }
            }
            
            context.Result = new ObjectResult(value)
            {
                StatusCode = statusCode
            };
            context.ExceptionHandled = true;
        }
    }
}

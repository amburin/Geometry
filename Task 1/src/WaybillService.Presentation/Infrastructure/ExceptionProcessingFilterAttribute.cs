using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using WaybillService.Core;
using WaybillService.Presentation.Controllers.Erros;

namespace WaybillService.Presentation.Infrastructure
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public sealed class ExceptionProcessingFilterAttribute : ActionFilterAttribute
    {
        private readonly ILogger _logger;
        private const string InternalErrorCode = "InternalError";

        public ExceptionProcessingFilterAttribute(ILogger<ExceptionProcessingFilterAttribute> logger)
        {
            _logger = logger;
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.ExceptionHandled)
            {
                return;
            }

            switch (context.Exception)
            {
                case BusinessException businessException:
                    ProcessBusinessException(context, businessException);
                    break;
                case Exception exception:
                    ProcessGeneralException(context, exception);
                    break;
                default:
                    return;
            }   
        }

        private void ProcessGeneralException(ActionExecutedContext context, Exception exception)
        {
            context.Result = new JsonResult(
                new ErrorResponse
                {
                    Error = new ErrorInfoResponse
                    {
                        Code = InternalErrorCode,
                        Message = exception.Message
                    },
                })
            {
                StatusCode = (int)HttpStatusCode.InternalServerError
            };
            context.ExceptionHandled = true;
            _logger.LogError(exception, "Unhandled exception {@ExceptionObject}", exception);
        }

        private void ProcessBusinessException(ActionExecutedContext context,
            BusinessException businessException)
        {
            var error = ErrorInfoResponseFactory.GetErrorResponse(businessException);
            context.Result = new BadRequestObjectResult(error);

            context.ExceptionHandled = true;
            _logger.LogWarning(businessException, "Unhandled business exception {@ExceptionObject}", businessException);
        }
    }
}
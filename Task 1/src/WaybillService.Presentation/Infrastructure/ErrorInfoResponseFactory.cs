using WaybillService.Core;
using WaybillService.Presentation.Controllers.Erros;

namespace WaybillService.Presentation.Infrastructure
{
    internal static class ErrorInfoResponseFactory
    {
        private const string InvalidInputCode = "InvalidInput";
        public static ErrorResponse GetErrorResponse(BusinessException businessException) =>
            new ErrorResponse
            {
                Error = new ErrorInfoResponse
                {
                    Code = InvalidInputCode,
                    Message = businessException.Message
                }
            };
    }
}
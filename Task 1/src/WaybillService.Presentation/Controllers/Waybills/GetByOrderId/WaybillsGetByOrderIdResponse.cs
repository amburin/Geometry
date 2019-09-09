using System;
using WaybillService.Presentation.Controllers.Waybills.GetByOrderId.ValidationResult;

namespace WaybillService.Presentation.Controllers.Waybills.GetByOrderId
{
    public class WaybillsGetByOrderIdResponse
    {
        public long SupplierWaybillId { get; set; }

        public WaybillsGetTypeResponse? Type { get; set; }
        public WaybillsGetSourceResponse Source { get; set; }
        public string DocumentNumber { get; set; }
        public DateTime? DocumentDate { get; set; }

        public string FileName { get; set; }
        public DateTimeOffset UploadTime { get; set; }
        
        public WaybillsGetValidationResultResponse ValidationResultResponse { get; set; }

        public WaybillsGetStatusResponse Status { get; set; }
    }
}
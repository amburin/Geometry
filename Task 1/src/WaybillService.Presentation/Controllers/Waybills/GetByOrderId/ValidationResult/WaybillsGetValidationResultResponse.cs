using System.Collections.Generic;

namespace WaybillService.Presentation.Controllers.Waybills.GetByOrderId.ValidationResult
{
    public class WaybillsGetValidationResultResponse
    {
        public bool HasConsigneeError { get; set; }
        public bool HasDocumentDateError { get; set; }
        public bool HasDocumentNumberError { get; set; }

        public bool HasEmptySumWithoutVat { get; set; }
        public bool HasEmptyTotalVatSum { get; set; }
        public bool HasEmptySumWithVat { get; set; }
        public bool HasInvalidSumWithoutVat { get; set; }
        public bool HasInvalidTotalVatSum { get; set; }
        public bool HasInvalidSumWithVat { get; set; }

        public IList<WaybillsGetItemValidationResultResponse> ItemValidationResults { get; set; }
        public bool HasErrors { get; set; }
        public int ErrorCount { get; set; }
    }
}
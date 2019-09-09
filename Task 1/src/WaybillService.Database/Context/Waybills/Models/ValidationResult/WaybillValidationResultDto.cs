using System.Collections.Generic;

namespace WaybillService.Database.Context.Waybills.Models.ValidationResult
{
    public class WaybillValidationResultDto
    {
        public long SupplierWaybillId { get; set; }

        public bool HasConsigneeError { get; set; }
        public bool HasDocumentDateError { get; set; }
        public bool HasDocumentNumberError { get; set; }
        public bool HasEmptySumWithoutVat { get; set; }
        public bool HasEmptyTotalVatSum { get; set; }
        public bool HasEmptySumWithVat { get; set; }
        public bool HasInvalidSumWithoutVat { get; set; }
        public bool HasInvalidTotalVatSum { get; set; }
        public bool HasInvalidSumWithVat { get; set; }

        public List<WaybillItemValidationResultDto> ItemValidationResults { get; set; }
    }
}
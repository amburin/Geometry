namespace WaybillService.Database.Context.Waybills.Models.ValidationResult
{
    public class WaybillItemValidationResultDto
    {
        public long WaybillId { get; set; }
        public long WaybillItemSequenceNumber { get; set; }

        public bool HasEmptyItemId { get; set; }
        public bool HasEmptyCode { get; set; }
        public bool HasEmptyName { get; set; }
        public bool HasEmptyOkeiUnitCode { get; set; }
        public bool HasEmptyUnitName { get; set; }
        public bool HasEmptyAmount { get; set; }
        public bool HasEmptyPriceWithoutVat { get; set; }
        public bool HasInvalidVatPercent { get; set; }
        public bool HasEmptySumWithVat { get; set; }
        public bool HasEmptySumWithoutVat { get; set; }
        public bool HasInvalidVatSum { get; set; }
        public bool HasInvalidSumWithVat { get; set; }
        public bool HasEmptyCustomsDeclarationNumber { get; set; }
        public bool HasEmptyCountryOfOriginCode { get; set; }
        public bool HasEmptyCountryOfOriginName { get; set; }
    }
}
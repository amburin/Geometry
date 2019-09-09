using System;

namespace WaybillService.Core.Waybills.Content.Header
{
    public class WaybillHeader
    {
        public string DocumentNumber { get; }
        public DateTime? DocumentDate { get; }
        public string Consignee { get; }

        public WaybillHeader(
            string documentNumber,
            DateTime? documentDate,
            string consignee)
        {
            DocumentNumber = documentNumber;
            DocumentDate = documentDate;
            Consignee = consignee;
        }

        public WaybillHeaderValidationResult Validate()
        {
            var hasConsigneeError = string.IsNullOrWhiteSpace(Consignee);

            var hasDocumentDateError =
                DocumentDate == null ||
                DocumentDate.Value == default;

            var hasDocumentNumberError =
                string.IsNullOrWhiteSpace(DocumentNumber);

            return new WaybillHeaderValidationResult(
                hasConsigneeError: hasConsigneeError,
                hasDocumentDateError: hasDocumentDateError,
                hasDocumentNumberError: hasDocumentNumberError
            );
        }
    }
}
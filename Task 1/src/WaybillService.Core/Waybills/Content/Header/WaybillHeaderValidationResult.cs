using System.Collections.Generic;
using System.Linq;

namespace WaybillService.Core.Waybills.Content.Header
{
    public class WaybillHeaderValidationResult : StructuralValidationResult
    {
        public bool HasConsigneeError { get; }
        public bool HasDocumentDateError { get; }
        public bool HasDocumentNumberError { get; }

        public WaybillHeaderValidationResult(
            bool hasConsigneeError,
            bool hasDocumentDateError,
            bool hasDocumentNumberError)
        {
            HasConsigneeError = hasConsigneeError;
            HasDocumentDateError = hasDocumentDateError;
            HasDocumentNumberError = hasDocumentNumberError;
        }

        protected override IEnumerable<bool> EnumerateErrorFlags()
        {
            yield return HasConsigneeError;
            yield return HasDocumentDateError;
            yield return HasDocumentNumberError;
        }

        protected override IEnumerable<StructuralValidationResult>
            EnumerateInnerValidationResults() => Enumerable.Empty<StructuralValidationResult>();
    }
}
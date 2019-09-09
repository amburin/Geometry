using System.Collections.Generic;
using System.Linq;

namespace WaybillService.Core.Waybills.Content.Item
{
    public class CustomsInfoValidationResult : StructuralValidationResult
    {
        public bool HasEmptyCustomDeclarationNumber { get; }
        public bool HasEmptyCountryOfOriginCode { get; }
        public bool HasEmptyCountryOfOriginName { get; }

        public CustomsInfoValidationResult(
            bool hasEmptyCustomDeclarationNumber,
            bool hasEmptyCountryOfOriginCode,
            bool hasEmptyCountryOfOriginName)
        {
            HasEmptyCustomDeclarationNumber = hasEmptyCustomDeclarationNumber;
            HasEmptyCountryOfOriginCode = hasEmptyCountryOfOriginCode;
            HasEmptyCountryOfOriginName = hasEmptyCountryOfOriginName;
        }

        protected override IEnumerable<bool> EnumerateErrorFlags()
        {
            yield return HasEmptyCustomDeclarationNumber;
            yield return HasEmptyCountryOfOriginCode;
            yield return HasEmptyCountryOfOriginName;
        }

        protected override IEnumerable<StructuralValidationResult>
            EnumerateInnerValidationResults() =>
            Enumerable.Empty<StructuralValidationResult>();
    }
}
using System.Collections.Generic;

namespace WaybillService.Core.Waybills.Content.Item
{
    public class WaybillItemValidationResult : StructuralValidationResult
    {
        public bool HasEmptyItemId { get; }
        public bool HasEmptyCode { get; }
        public bool HasEmptyName { get; }
        public bool HasEmptyOkeiUnitCode { get; }
        public bool HasEmptyUnitName { get; }
        public MonetaryValueValidationResult MonetaryValueValidationResult { get; }
        public CustomsInfoValidationResult CustomsInfoValidationResult { get; }

        public WaybillItemValidationResult(
            bool hasEmptyItemId,
            bool hasEmptyCode,
            bool hasEmptyName,
            bool hasEmptyOkeiUnitCode,
            bool hasEmptyUnitName,
            MonetaryValueValidationResult monetaryMonetaryValueValidationResult,
            CustomsInfoValidationResult customsInfoValidationResult)
        {
            MonetaryValueValidationResult = monetaryMonetaryValueValidationResult;
            HasEmptyItemId = hasEmptyItemId;
            HasEmptyCode = hasEmptyCode;
            HasEmptyName = hasEmptyName;
            HasEmptyOkeiUnitCode = hasEmptyOkeiUnitCode;
            HasEmptyUnitName = hasEmptyUnitName;
            CustomsInfoValidationResult = customsInfoValidationResult;
        }

        protected override IEnumerable<bool> EnumerateErrorFlags()
        {
            yield return HasEmptyItemId;
            yield return HasEmptyCode;
            yield return HasEmptyName;
            yield return HasEmptyOkeiUnitCode;
            yield return HasEmptyUnitName;
        }

        protected override IEnumerable<StructuralValidationResult> EnumerateInnerValidationResults()
        {
            yield return MonetaryValueValidationResult;
            yield return CustomsInfoValidationResult;
        }
    }
}
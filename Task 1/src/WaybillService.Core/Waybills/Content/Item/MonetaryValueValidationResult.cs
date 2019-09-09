using System.Collections.Generic;
using System.Linq;

namespace WaybillService.Core.Waybills.Content.Item
{
    public class MonetaryValueValidationResult : StructuralValidationResult
    {
        public bool HasEmptyAmount { get; }
        public bool HasEmptyPriceWithoutVat { get; }
        public bool HasInvalidVatPercent { get; }
        public bool HasEmptySumWithVat { get; }
        public bool HasEmptySumWithoutVat { get; }
        public bool HasInvalidVatSum { get; }
        public bool HasInvalidSumWithVat { get; }

        public MonetaryValueValidationResult(
            bool hasEmptyAmount,
            bool hasEmptyPriceWithoutVat,
            bool hasInvalidVatPercent,
            bool hasEmptySumWithVat,
            bool hasEmptySumWithoutVat,
            bool hasInvalidVatSum,
            bool hasInvalidSumWithVat)
        {
            HasEmptyAmount = hasEmptyAmount;
            HasEmptyPriceWithoutVat = hasEmptyPriceWithoutVat;
            HasInvalidVatPercent = hasInvalidVatPercent;
            HasEmptySumWithVat = hasEmptySumWithVat;
            HasEmptySumWithoutVat = hasEmptySumWithoutVat;
            HasInvalidVatSum = hasInvalidVatSum;
            HasInvalidSumWithVat = hasInvalidSumWithVat;
        }

        protected override IEnumerable<bool> EnumerateErrorFlags()
        {
            yield return HasEmptyAmount;
            yield return HasEmptyPriceWithoutVat;
            yield return HasInvalidVatPercent;
            yield return HasEmptySumWithVat;
            yield return HasEmptySumWithoutVat;
            yield return HasInvalidVatSum;  
            yield return HasInvalidSumWithVat;
        }

        protected override IEnumerable<StructuralValidationResult> EnumerateInnerValidationResults()
            => Enumerable.Empty<StructuralValidationResult>();
    }
}
using System.Collections.Generic;
using System.Linq;

namespace WaybillService.Core.Waybills.Content.Totals
{
    public class WaybillTotalsValidationResult : StructuralValidationResult
    {
        public bool HasEmptySumWithoutVat { get; }
        public bool HasEmptyTotalVatSum { get; }
        public bool HasEmptySumWithVat { get; }
        public bool HasInvalidSumWithoutVat { get; }
        public bool HasInvalidTotalVatSum { get; }
        public bool HasInvalidSumWithVat { get; }

        public WaybillTotalsValidationResult(
            bool hasEmptySumWithoutVat,
            bool hasEmptyTotalVatSum,
            bool hasEmptySumWithVat,
            bool hasInvalidTotalVatSum,
            bool hasInvalidSumWithoutVat,
            bool hasInvalidSumWithVat)
        {
            HasEmptySumWithoutVat = hasEmptySumWithoutVat;
            HasEmptyTotalVatSum = hasEmptyTotalVatSum;
            HasEmptySumWithVat = hasEmptySumWithVat;
            HasInvalidSumWithoutVat = hasInvalidSumWithoutVat;
            HasInvalidTotalVatSum = hasInvalidTotalVatSum;
            HasInvalidSumWithVat = hasInvalidSumWithVat;
        }

        protected override IEnumerable<bool> EnumerateErrorFlags()
        {
            yield return HasEmptySumWithoutVat;
            yield return HasEmptyTotalVatSum;
            yield return HasEmptySumWithVat;
            yield return HasInvalidSumWithoutVat;
            yield return HasInvalidTotalVatSum;
            yield return HasInvalidSumWithVat;
        }

        protected override IEnumerable<StructuralValidationResult> EnumerateInnerValidationResults()
            => Enumerable.Empty<StructuralValidationResult>();
    }
}
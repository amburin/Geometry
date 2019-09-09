using System;
using System.Collections.Generic;
using System.Linq;
using WaybillService.Core.Waybills.Content.Header;
using WaybillService.Core.Waybills.Content.Item;
using WaybillService.Core.Waybills.Content.Totals;

namespace WaybillService.Core.Waybills.Content
{
    public class WaybillContentStructuralValidationResult : StructuralValidationResult
    {
        public WaybillHeaderValidationResult HeaderResult { get; }
        public IReadOnlyList<WaybillItemValidationResult> ItemResults { get; }
        public WaybillTotalsValidationResult TotalsResult { get; }

        public WaybillContentStructuralValidationResult(
            WaybillHeaderValidationResult headerResult,
            IReadOnlyList<WaybillItemValidationResult> itemResults,
            WaybillTotalsValidationResult totalsResult)
        {
            HeaderResult = headerResult
                ?? throw new ArgumentNullException(nameof(headerResult));
            ItemResults = itemResults
                ?? throw new ArgumentNullException(nameof(itemResults));
            TotalsResult = totalsResult
                ?? throw new ArgumentNullException(nameof(totalsResult));
        }

        protected override IEnumerable<bool> EnumerateErrorFlags() => Enumerable.Empty<bool>();

        protected override IEnumerable<StructuralValidationResult> EnumerateInnerValidationResults()
        {
            yield return HeaderResult;
            
            foreach (var itemValidationResult in ItemResults)
            {
                yield return itemValidationResult;
            }

            yield return TotalsResult;
        }
    }
}
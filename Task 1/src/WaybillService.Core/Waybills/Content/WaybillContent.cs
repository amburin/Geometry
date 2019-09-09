using System;
using System.Collections.Generic;
using System.Linq;
using WaybillService.Core.Waybills.Content.Header;
using WaybillService.Core.Waybills.Content.Item;
using WaybillService.Core.Waybills.Content.Totals;

namespace WaybillService.Core.Waybills.Content
{
    public class WaybillContent
    {
        public WaybillHeader Header { get; }
        public IReadOnlyCollection<WaybillItem> Items { get; }
        public WaybillTotals Totals { get; }

        public WaybillContent(
            WaybillHeader header,
            IReadOnlyCollection<WaybillItem> items,
            WaybillTotals totals)
        {
            Header = header ?? throw new ArgumentNullException(nameof(header));
            Items = items ?? throw new ArgumentNullException(nameof(items));
            Totals = totals ?? throw new ArgumentNullException(nameof(totals));
        }

        public WaybillContentStructuralValidationResult Validate()
        {
            var headerValidationResult = Header.Validate();

            var itemValidationResults = Items
                .Select(x => x.Validate())
                .ToArray();

            var totalsValidationResult = ValidateTotals(Totals, Items);

            return new WaybillContentStructuralValidationResult(
                headerValidationResult,
                itemValidationResults,
                totalsValidationResult
            );
        }

        private static WaybillTotalsValidationResult ValidateTotals(
            WaybillTotals totals,
            IReadOnlyCollection<WaybillItem> waybillItems)
        {
            var hasEmptyTotalVatSum = totals.TotalVatSum.GetValueOrDefault() == 0;
            var hasEmptyTotalSumWithVat = totals.TotalSumWithVat.GetValueOrDefault() == 0;
            var hasEmptyTotalSumWithoutVat = totals.TotalSumWithoutVat.GetValueOrDefault() == 0;

            var sumWithoutVat = waybillItems.Sum(x => x.MonetaryValue.SumWithoutVat ?? 0);
            var sumWithVat = waybillItems.Sum(x => x.MonetaryValue.SumWithVat ?? 0);
            var sumVat = waybillItems.Sum(x => x.MonetaryValue.VatSum ?? 0);

            return new WaybillTotalsValidationResult(
                hasEmptySumWithoutVat: hasEmptyTotalSumWithoutVat,
                hasEmptyTotalVatSum: hasEmptyTotalVatSum,
                hasEmptySumWithVat: hasEmptyTotalSumWithVat,
                hasInvalidSumWithoutVat: sumWithoutVat != totals.TotalSumWithoutVat,
                hasInvalidTotalVatSum: sumVat != totals.TotalSumWithVat,
                hasInvalidSumWithVat: sumWithVat != totals.TotalSumWithVat);
        }
    }
}
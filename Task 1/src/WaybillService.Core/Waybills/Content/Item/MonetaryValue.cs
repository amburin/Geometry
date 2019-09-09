using System.Linq;

namespace WaybillService.Core.Waybills.Content.Item
{
    public class MonetaryValue
    {
        public decimal? SumWithoutVat { get; }
        public decimal? VatSum { get; }
        public decimal? SumWithVat { get; }
        public int? Amount { get; }
        public decimal? PriceWithoutVat { get; }
        public decimal? VatPercent { get; }

        public MonetaryValue(
            int? amount,
            decimal? priceWithoutVat,
            decimal? vatPercent,
            decimal? sumWithoutVat,
            decimal? vatSum,
            decimal? sumWithVat)
        {
            Amount = amount;
            PriceWithoutVat = priceWithoutVat;
            VatPercent = vatPercent;
            SumWithVat = sumWithVat;
            SumWithoutVat = sumWithoutVat;
            VatSum = vatSum;
        }

        public MonetaryValueValidationResult Validate()
        {
            return new MonetaryValueValidationResult(
                hasEmptyAmount: Amount.GetValueOrDefault() == 0,
                hasEmptyPriceWithoutVat: PriceWithoutVat.GetValueOrDefault() == 0,
                hasInvalidVatPercent: !IsVatPercentValid(),
                hasEmptySumWithVat: VatSum.GetValueOrDefault() == 0,
                hasEmptySumWithoutVat: SumWithoutVat.GetValueOrDefault() == 0,
                hasInvalidVatSum: !DoesVatSumMatchWithSumWithoutVat(),
                hasInvalidSumWithVat: !DoesSumWithVatMatchSumWithoutVat());
        }

        private bool DoesSumWithVatMatchSumWithoutVat()
        {
            var calculatedSumWithVat = SumWithoutVat * ((100 + VatPercent) / 100);
            return calculatedSumWithVat == SumWithVat;
        }

        private bool IsVatPercentValid()
        {
            var validVatValues = new[] {0m, 10m, 20m};
            return VatPercent.HasValue && validVatValues.Contains(VatPercent.Value);
        }

        private bool DoesVatSumMatchWithSumWithoutVat()
        {
            var calculatedVatSum = SumWithoutVat * VatPercent / 100;
            return calculatedVatSum == VatSum;
        }
    }
}
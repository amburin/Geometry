namespace WaybillService.Core.Waybills.Content.Totals
{
    public class WaybillTotals
    {
        public decimal? TotalSumWithoutVat { get; }
        public decimal? TotalVatSum { get; }
        public decimal? TotalSumWithVat { get; }

        public WaybillTotals(
            decimal? totalSumWithoutVat,
            decimal? totalVatSum,
            decimal? totalSumWithVat)
        {
            TotalSumWithoutVat = totalSumWithoutVat;
            TotalVatSum = totalVatSum;
            TotalSumWithVat = totalSumWithVat;
        }
    }
}
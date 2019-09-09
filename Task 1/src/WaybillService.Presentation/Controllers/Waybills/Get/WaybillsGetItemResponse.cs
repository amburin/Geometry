namespace WaybillService.Presentation.Controllers.Waybills.Get
{
    public class WaybillsGetItemResponse
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public long? RezonItemId { get; set; }

        public int? Amount { get; set; }
        public decimal? PriceWithoutVat { get; set; }

        public decimal? SumWithoutVat { get; set; }
        public decimal? SumWithVat { get; set; }
        public decimal? SumVat { get; set; }
    }
}
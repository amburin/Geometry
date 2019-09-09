
namespace WaybillService.Database.Context.Waybills.Models
{
    public class WaybillItemDto
    {
        public int SequenceNumber { get; set; }
        public long SupplierWaybillId { get; set; }

        public long? RezonItemId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Unit { get; set; }
        public string UnitOkeiCode { get; set; }
        public int? Amount { get; set; }

        public decimal? PriceWithoutVat { get; set; }
        public decimal? VatPercent { get; set; }
        public decimal? VatSum { get; set; }
        public decimal? SumWithoutVat { get; set; }
        public decimal? SumWithVat { get; set; }

        public string CountryOfOriginCode { get; set; }
        public string CountryOfOriginName { get; set; }
        public string CustomsDeclarationNumber { get; set; }
    }
}
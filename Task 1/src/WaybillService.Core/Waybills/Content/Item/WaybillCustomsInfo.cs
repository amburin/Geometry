namespace WaybillService.Core.Waybills.Content.Item
{
    public class WaybillCustomsInfo
    {
        public string CountryOfOriginCode { get; }
        public string CountryOfOriginName { get; }
        public string CustomsDeclarationNumber { get; }

        public WaybillCustomsInfo(
            string countryOfOriginName,
            string customsDeclarationNumber,
            string countryOfOriginCode)
        {
            CountryOfOriginName = countryOfOriginName;
            CustomsDeclarationNumber = customsDeclarationNumber;
            CountryOfOriginCode = countryOfOriginCode;
        }

        public CustomsInfoValidationResult Validate()
        {
            return new CustomsInfoValidationResult(
                string.IsNullOrWhiteSpace(CustomsDeclarationNumber),
                string.IsNullOrWhiteSpace(CountryOfOriginCode),
                string.IsNullOrWhiteSpace(CountryOfOriginName));
        }
    }
}
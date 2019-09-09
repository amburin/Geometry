namespace WaybillService.Core.Waybills.Content.Item
{
    public class WaybillItem
    {
        public ItemId ItemId { get; }
        public string Code { get; }
        public string Name { get; }
        public Unit Unit { get; }
        public WaybillCustomsInfo CustomsInfo { get; }
        public MonetaryValue MonetaryValue { get; }

        public WaybillItem(
            ItemId itemId,
            string name,
            string code,
            Unit unit,
            MonetaryValue monetaryValue,
            WaybillCustomsInfo customsInfo)
        {
            ItemId = itemId;
            Name = name;
            Code = code;
            Unit = unit;
            CustomsInfo = customsInfo;
            MonetaryValue = monetaryValue;
        }

        public WaybillItemValidationResult Validate()
        {
            var monetaryValueValidationResult = MonetaryValue.Validate();
            var customsInfoValidationResult = CustomsInfo.Validate();

            return new WaybillItemValidationResult(
                hasEmptyItemId: ItemId == null,
                hasEmptyCode: string.IsNullOrWhiteSpace(Code),
                hasEmptyName: string.IsNullOrWhiteSpace(Name),
                hasEmptyOkeiUnitCode: string.IsNullOrWhiteSpace(Unit.OkeiCode),
                hasEmptyUnitName: string.IsNullOrWhiteSpace(Unit.Name),
                monetaryValueValidationResult,
                customsInfoValidationResult
            );
        }
    }
}
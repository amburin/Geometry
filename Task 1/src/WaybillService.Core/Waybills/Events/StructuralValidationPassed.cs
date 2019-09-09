namespace WaybillService.Core.Waybills.Events
{
    public class StructuralValidationPassed : DomainEvent
    {
        public WaybillId WaybillId { get; }

        public StructuralValidationPassed(WaybillId waybillId)
        {
            WaybillId = waybillId;
        }
    }
}
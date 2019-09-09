namespace WaybillService.Core.Waybills.Events
{
    public class WaybillParsed : DomainEvent
    {
        public WaybillId WaybillId { get; }

        public WaybillParsed(WaybillId waybillId)
        {
            WaybillId = waybillId;
        }
    }
}
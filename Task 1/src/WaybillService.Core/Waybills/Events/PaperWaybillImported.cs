namespace WaybillService.Core.Waybills.Events
{
    public class PaperWaybillImported : DomainEvent
    {
        public WaybillId WaybillId { get; }

        public PaperWaybillImported(WaybillId waybillId)
        {
            WaybillId = waybillId;
        }
    }
}
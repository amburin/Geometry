namespace WaybillService.Core.Waybills
{
    public class WaybillId : LongBasedId<WaybillId>
    {
        public WaybillId(long value) : base(value)
        {
        }
    }
}
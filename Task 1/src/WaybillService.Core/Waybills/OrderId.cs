namespace WaybillService.Core.Waybills
{
    public class OrderId : LongBasedId<OrderId>
    {
        public OrderId(long value) : base(value)
        {
        }
    }
}
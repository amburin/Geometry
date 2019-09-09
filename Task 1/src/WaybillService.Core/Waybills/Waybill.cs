using WaybillService.Core.Waybills.Content;

namespace WaybillService.Core.Waybills
{
    public abstract class Waybill
    {
        protected Waybill(OrderId orderId)
        {
            OrderId = orderId;
        }

        /// <summary>Ctor for mapping from DB exclusively.</summary>
        protected Waybill(
            WaybillId id,
            OrderId orderId,
            WaybillContent content)
        {
            Id = id;
            OrderId = orderId;
            Content = content;
        }

        public WaybillId Id { get; protected set; }
        public OrderId OrderId { get; }
        public WaybillContent Content { get; protected set; }
        public abstract WaybillStatus Status { get; }
        public abstract bool IsEdo { get; }

        public abstract  void SetId(WaybillId id);
    }
}
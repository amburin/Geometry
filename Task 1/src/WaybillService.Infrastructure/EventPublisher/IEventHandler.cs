using WaybillService.Core;

namespace WaybillService.Infrastructure.EventPublisher
{
    public interface IHandler<TEvent> where TEvent : DomainEvent
    {
        void Handle(TEvent businessEvent);
    }
}
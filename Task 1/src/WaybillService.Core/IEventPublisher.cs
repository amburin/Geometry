using System.Runtime.CompilerServices;

namespace WaybillService.Core
{
    public interface IEventPublisher
    {
        void Raise<TEvent>(TEvent eventData, [CallerMemberName] string callerName = "")
            where TEvent : DomainEvent;
    }
}
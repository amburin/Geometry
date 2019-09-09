using Microsoft.Extensions.DependencyInjection;
using WaybillService.Core;
using WaybillService.Infrastructure.EventPublisher;

namespace WaybillService.Infrastructure
{
    public class EventHandlersBundle<TDomainEvent> where TDomainEvent : DomainEvent
    {
        private readonly IServiceCollection _serviceCollection;

        public EventHandlersBundle(IServiceCollection serviceCollection)
        {
            _serviceCollection = serviceCollection;
        }

        public EventHandlersBundle<TDomainEvent> WithHandler<THandler>()
            where THandler : class, IHandler<TDomainEvent>
        {
            _serviceCollection.AddScoped<IHandler<TDomainEvent>, THandler>();

            return new EventHandlersBundle<TDomainEvent>(_serviceCollection);
        }
    }
}
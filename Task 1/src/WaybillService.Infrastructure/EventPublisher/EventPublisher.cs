using System;
using System.Linq;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WaybillService.Core;

namespace WaybillService.Infrastructure.EventPublisher
{
    public class EventPublisher : IEventPublisher
    {
        private readonly IServiceProvider _provider;
        private readonly ILogger _logger;

        public EventPublisher(IServiceProvider provider, ILogger<EventPublisher> logger)
        {
            _provider = provider;
            _logger = logger;
        }

        public void Raise<TEvent>(TEvent eventData, [CallerMemberName] string callerName = "")
            where TEvent : DomainEvent
        {
            _logger.LogInformation(
                "Event {0} raised by {1} with eventData {@eventData}.",
                typeof(TEvent),
                callerName,
                eventData);

            var handlers = _provider.GetServices<IHandler<TEvent>>().ToArray();

            if (!handlers.Any())
            {
                _logger.LogWarning("Event handler for event type {0} not found.", typeof(TEvent));
            }

            foreach (var handler in handlers)
            {
                _logger.LogInformation(
                    "Calling {0} handler for {1} event.",
                    handler.GetType(),
                    typeof(TEvent));

                handler.Handle(eventData);
            }
        }
    }
}
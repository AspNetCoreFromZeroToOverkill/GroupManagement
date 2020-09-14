using System.Threading;
using System.Threading.Tasks;
using CodingMilitia.PlayBall.Auth.Events;
using CodingMilitia.PlayBall.GroupManagement.Infrastructure.Events;
using CodingMilitia.PlayBall.Shared.EventBus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CodingMilitia.PlayBall.GroupManagement.Web.BackgroundWorkers
{
    public class EventConsumerBackgroundService : BackgroundService
    {
        private readonly IEventConsumer<BaseUserEvent> _eventConsumer;
        private readonly IServiceScopeFactory _scopeFactory;

        public EventConsumerBackgroundService(IEventConsumer<BaseUserEvent> eventConsumer, IServiceScopeFactory scopeFactory)
        {
            _eventConsumer = eventConsumer;
            _scopeFactory = scopeFactory;
        }
        
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
            => _eventConsumer.Subscribe(@event => ForwardEventAsync(@event, stoppingToken), stoppingToken);

        private async Task ForwardEventAsync(BaseUserEvent @event, CancellationToken ct)
        {
            using var scope = _scopeFactory.CreateScope();
            var eventHandler = scope.ServiceProvider.GetRequiredService<IEventHandler<BaseUserEvent>>();
            await eventHandler.HandleAsync(@event);
        }
    }
}
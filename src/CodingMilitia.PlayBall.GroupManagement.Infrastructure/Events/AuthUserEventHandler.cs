using System;
using System.Threading.Tasks;
using CodingMilitia.PlayBall.Auth.Events;
using Microsoft.Extensions.Logging;

namespace CodingMilitia.PlayBall.GroupManagement.Infrastructure.Events
{
    public class AuthUserEventHandler : IEventHandler<BaseUserEvent>
    {
        private readonly ILogger<BaseUserEvent> _logger;

        public AuthUserEventHandler(ILogger<BaseUserEvent> logger)
        {
            _logger = logger;
        }

        public Task HandleAsync(BaseUserEvent @event)
            => @event switch
            {
                UserRegisteredEvent registered => CreateUserAsync(registered),
                UserUpdatedEvent updated => UpdateUserAsync(updated),
                UserDeletedEvent deleted => DeleteUserAsync(deleted),
                _ => LogUnhandledEvent(@event)
            };

        private Task CreateUserAsync(UserRegisteredEvent @event)
        {
            _logger.LogInformation("User {userId} created with username {userName}", @event.UserId, @event.UserName);
            return Task.CompletedTask;
        }

        private Task UpdateUserAsync(UserUpdatedEvent @event)
        {
            _logger.LogInformation("User {userId} updated with username {userName}", @event.UserId, @event.UserName);
            return Task.CompletedTask;
        }

        Task DeleteUserAsync(UserDeletedEvent @event)
        {
            _logger.LogInformation("User {userId} deleted", @event.UserId);
            return Task.CompletedTask;
        }

        private Task LogUnhandledEvent(BaseUserEvent @event)
        {
            _logger.LogDebug("Received unhandled event of type {eventType}", @event.GetType().Name);
            return Task.CompletedTask;
        }
    }
}
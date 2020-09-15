using System;
using System.Threading;
using System.Threading.Tasks;
using CodingMilitia.PlayBall.Auth.Events;
using CodingMilitia.PlayBall.GroupManagement.Domain.Entities;
using CodingMilitia.PlayBall.GroupManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CodingMilitia.PlayBall.GroupManagement.Infrastructure.Events
{
    public class AuthUserEventHandler : IEventHandler<BaseUserEvent>
    {
        private readonly GroupManagementDbContext _db;
        private readonly ILogger<BaseUserEvent> _logger;

        public AuthUserEventHandler(GroupManagementDbContext db, ILogger<BaseUserEvent> logger)
        {
            _db = db;
            _logger = logger;
        }

        public Task HandleAsync(BaseUserEvent @event, CancellationToken ct)
            => @event switch
            {
                UserRegisteredEvent registered => CreateUserAsync(registered, ct),
                UserUpdatedEvent updated => UpdateUserAsync(updated, ct),
                UserDeletedEvent deleted => DeleteUserAsync(deleted, ct),
                _ => LogUnhandledEvent(@event)
            };

        private async Task CreateUserAsync(UserRegisteredEvent @event, CancellationToken ct)
        {
            await _db.Set<User>().AddAsync(new User(@event.UserId, @event.UserName), ct);
            await _db.SaveChangesAsync(ct);
        }

        private async Task UpdateUserAsync(UserUpdatedEvent @event, CancellationToken ct)
        {
            var updatedRows = await _db.Database.ExecuteSqlInterpolatedAsync(
                $"UPDATE \"Users\" SET \"Name\" = {@event.UserName} WHERE \"Id\" = {@event.UserId}",
                ct);

            if (updatedRows != 1)
            {
                // TODO: instead of throwing, consider a return type with success and failure options
                // TODO: also, Exception base class shouldn't be used 🙃
                throw new Exception($"Expected to update a user row, updated {updatedRows} instead.");
            }
        }

        private async Task DeleteUserAsync(UserDeletedEvent @event, CancellationToken ct)
        {
            var deletedRows = await _db.Database.ExecuteSqlInterpolatedAsync(
                $"DELETE FROM \"Users\" WHERE \"Id\" = {@event.UserId}",
                ct);

            if (deletedRows != 1)
            {
                // TODO: instead of throwing, consider a return type with success and failure options
                // TODO: also, Exception base class shouldn't be used 🙃
                throw new Exception($"Expected to delete a user row, deleted {deletedRows} instead.");
            }
        }

        private Task LogUnhandledEvent(BaseUserEvent @event)
        {
            _logger.LogDebug("Received unhandled event of type {eventType}", @event.GetType().Name);
            return Task.CompletedTask;
        }
    }
}
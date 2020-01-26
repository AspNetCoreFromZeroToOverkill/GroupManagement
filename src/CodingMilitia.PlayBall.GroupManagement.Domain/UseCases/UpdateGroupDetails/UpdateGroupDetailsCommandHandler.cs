using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CodingMilitia.PlayBall.GroupManagement.Domain.Data;
using CodingMilitia.PlayBall.GroupManagement.Domain.Data.Queries;
using CodingMilitia.PlayBall.GroupManagement.Domain.Entities;
using CodingMilitia.PlayBall.GroupManagement.Domain.Shared;
using MediatR;

namespace CodingMilitia.PlayBall.GroupManagement.Domain.UseCases.UpdateGroupDetails
{
    public sealed class UpdateGroupDetailsCommandHandler
        : IRequestHandler<UpdateGroupDetailsCommand, Optional<UpdateGroupDetailsCommandResult>>
    {
        private readonly IQueryHandler<UserByIdQuery, Optional<User>> _userByIdQueryHandler;
        private readonly IQueryHandler<UserGroupQuery, Optional<Group>> _userGroupQueryHandler;
        private readonly IVersionedRepository<Group, uint> _groupsRepository;

        public UpdateGroupDetailsCommandHandler(
            IQueryHandler<UserByIdQuery, Optional<User>> userByIdQueryHandler,
            IQueryHandler<UserGroupQuery, Optional<Group>> userGroupQueryHandler,
            IVersionedRepository<Group, uint> groupsRepository)
        {
            _userByIdQueryHandler =
                userByIdQueryHandler ?? throw new ArgumentNullException(nameof(userByIdQueryHandler));
            _userGroupQueryHandler =
                userGroupQueryHandler ?? throw new ArgumentNullException(nameof(userGroupQueryHandler));
            _groupsRepository = groupsRepository ?? throw new ArgumentNullException(nameof(groupsRepository));
        }

        public async Task<Optional<UpdateGroupDetailsCommandResult>> Handle(
            UpdateGroupDetailsCommand request,
            CancellationToken cancellationToken)
        {
            var maybeGroup = await _userGroupQueryHandler.HandleAsync(
                new UserGroupQuery(request.UserId, request.GroupId),
                cancellationToken);

            if (!maybeGroup.TryGetValue(out var group))
            {
                return Optional.None<UpdateGroupDetailsCommandResult>();
            }

            var maybeUser = await _userByIdQueryHandler.HandleAsync(
                new UserByIdQuery(request.UserId),
                cancellationToken);

            if (!maybeUser.TryGetValue(out var currentUser))
            {
                // TODO: we'll get rid of these exceptions in the next episode
                throw new InvalidOperationException("Invalid user to create a group.");
            }

            group.Rename(currentUser, request.Name);

            await _groupsRepository.UpdateAsync(group, uint.Parse(request.RowVersion), cancellationToken);

            return Optional.Some(new UpdateGroupDetailsCommandResult(
                group.Id,
                group.Name,
                group.RowVersion.ToString()));
        }
    }
}
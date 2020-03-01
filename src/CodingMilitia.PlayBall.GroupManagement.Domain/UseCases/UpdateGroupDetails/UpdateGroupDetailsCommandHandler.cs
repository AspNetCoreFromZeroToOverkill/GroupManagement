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
        : IRequestHandler<UpdateGroupDetailsCommand, Either<Error, UpdateGroupDetailsCommandResult>>
    {
        private readonly IVersionedRepository<Group, uint> _groupsRepository;
        private readonly IQueryHandler<UserByIdQuery, Optional<User>> _userByIdQueryHandler;
        private readonly IQueryHandler<UserGroupQuery, Optional<Group>> _userGroupQueryHandler;

        public UpdateGroupDetailsCommandHandler(
            IQueryHandler<UserByIdQuery, Optional<User>> userByIdQueryHandler,
            IQueryHandler<UserGroupQuery, Optional<Group>> userGroupQueryHandler,
            IVersionedRepository<Group, uint> groupsRepository)
        {
            _userByIdQueryHandler = Ensure.NotNull(userByIdQueryHandler, nameof(userByIdQueryHandler));
            _userGroupQueryHandler = Ensure.NotNull(userGroupQueryHandler, nameof(userGroupQueryHandler));
            _groupsRepository = Ensure.NotNull(groupsRepository, nameof(groupsRepository));
        }

        public async Task<Either<Error, UpdateGroupDetailsCommandResult>> Handle(
            UpdateGroupDetailsCommand request,
            CancellationToken cancellationToken)
        {
            var maybeGroup = await _userGroupQueryHandler.HandleAsync(
                new UserGroupQuery(request.UserId, request.GroupId),
                cancellationToken);

            if (!maybeGroup.TryGetValue(out var group))
            {
                return Result.NotFound<UpdateGroupDetailsCommandResult>(
                    $"Group with id {request.GroupId} not found.");
            }


            var maybeUser = await _userByIdQueryHandler.HandleAsync(
                new UserByIdQuery(request.UserId),
                cancellationToken);

            if (!maybeUser.TryGetValue(out var currentUser))
            {
                return Result.Invalid<UpdateGroupDetailsCommandResult>(
                    "Invalid user to create a group.");
            }

            return await group
                .Rename(currentUser, request.Name)
                .MapAsync(async _ =>
                {
                    await _groupsRepository.UpdateAsync(
                        group,
                        uint.Parse(request.RowVersion),
                        cancellationToken);

                    return new UpdateGroupDetailsCommandResult(
                        group.Id,
                        group.Name,
                        group.RowVersion.ToString());
                });
        }
    }
}
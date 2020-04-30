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
        private readonly QueryRunner<UserByIdQuery, Optional<User>> _getUserById;
        private readonly QueryRunner<UserGroupQuery, Optional<Group>> _getUserGroup;

        public UpdateGroupDetailsCommandHandler(
            QueryRunner<UserByIdQuery, Optional<User>> getUserById,
            QueryRunner<UserGroupQuery, Optional<Group>> getUserGroup,
            IVersionedRepository<Group, uint> groupsRepository)
        {
            _getUserById = Ensure.NotNull(getUserById, nameof(getUserById));
            _getUserGroup = Ensure.NotNull(getUserGroup, nameof(getUserGroup));
            _groupsRepository = Ensure.NotNull(groupsRepository, nameof(groupsRepository));
        }

        public async Task<Either<Error, UpdateGroupDetailsCommandResult>> Handle(
            UpdateGroupDetailsCommand request,
            CancellationToken cancellationToken)
        {
            var maybeGroup = await _getUserGroup(
                new UserGroupQuery(request.UserId, request.GroupId),
                cancellationToken);

            if (!maybeGroup.TryGetValue(out var group))
            {
                return Result.NotFound<UpdateGroupDetailsCommandResult>(
                    $"Group with id {request.GroupId} not found.");
            }


            var maybeUser = await _getUserById(
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
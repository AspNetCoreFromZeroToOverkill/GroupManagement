using System.Threading;
using System.Threading.Tasks;
using CodingMilitia.PlayBall.GroupManagement.Domain.Data;
using CodingMilitia.PlayBall.GroupManagement.Domain.Data.Queries;
using CodingMilitia.PlayBall.GroupManagement.Domain.Entities;
using CodingMilitia.PlayBall.GroupManagement.Domain.Shared;
using MediatR;

namespace CodingMilitia.PlayBall.GroupManagement.Domain.UseCases.DeleteGroup
{
    public class DeleteGroupCommandHandler : IRequestHandler<DeleteGroupCommand, Unit>
    {
        private readonly IVersionedRepository<Group, uint> _groupsRepository;
        private readonly QueryRunner<UserGroupQuery, Optional<Group>> _getUserGroup;

        public DeleteGroupCommandHandler(
            QueryRunner<UserGroupQuery, Optional<Group>> getUserGroup,
            IVersionedRepository<Group, uint> groupsRepository)
        {
            _getUserGroup = Ensure.NotNull(getUserGroup, nameof(getUserGroup));
            _groupsRepository = Ensure.NotNull(groupsRepository, nameof(groupsRepository));
        }

        public async Task<Unit> Handle(DeleteGroupCommand request, CancellationToken cancellationToken)
        {
            var maybeGroup = await _getUserGroup(
                new UserGroupQuery(request.UserId, request.GroupId),
                cancellationToken);

            await maybeGroup.MatchSomeAsync(async group =>
            {
                if (group.IsAdmin(request.UserId))
                {
                    await _groupsRepository.DeleteAsync(@group, cancellationToken);
                }
            });

            return Unit.Value;
        }
    }
}
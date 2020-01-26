using System;
using System.Linq;
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
        private readonly IQueryHandler<UserGroupQuery, Optional<Group>> _userGroupQueryHandler;
        private readonly IVersionedRepository<Group, uint> _groupsRepository;

        public DeleteGroupCommandHandler(
            IQueryHandler<UserGroupQuery, Optional<Group>> userGroupQueryHandler,
            IVersionedRepository<Group, uint> groupsRepository)
        {
            _userGroupQueryHandler =
                userGroupQueryHandler ?? throw new ArgumentNullException(nameof(userGroupQueryHandler));
            _groupsRepository = groupsRepository ?? throw new ArgumentNullException(nameof(groupsRepository));
        }

        public async Task<Unit> Handle(DeleteGroupCommand request, CancellationToken cancellationToken)
        {
            var maybeGroup = await _userGroupQueryHandler.HandleAsync(
                new UserGroupQuery(request.UserId, request.GroupId),
                cancellationToken);

            await maybeGroup.MatchSomeAsync(async group =>
            {
                if (group.IsAdmin(request.UserId))
                {
                    await _groupsRepository.DeleteAsync(group, cancellationToken);
                }
            });

            return Unit.Value;
        }
    }
}
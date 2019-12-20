using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CodingMilitia.PlayBall.GroupManagement.Domain.Data;
using CodingMilitia.PlayBall.GroupManagement.Domain.Data.Queries;
using CodingMilitia.PlayBall.GroupManagement.Domain.Entities;
using MediatR;

namespace CodingMilitia.PlayBall.GroupManagement.Domain.UseCases.UpdateGroupDetails
{
    public sealed class UpdateGroupDetailsCommandHandler
        : IRequestHandler<UpdateGroupDetailsCommand, UpdateGroupDetailsCommandResult>
    {
        private readonly IQueryHandler<UserGroupQuery, Group> _userGroupQueryHandler;
        private readonly IRepository<Group> _groupsRepository;

        public UpdateGroupDetailsCommandHandler(
            IQueryHandler<UserGroupQuery, Group> userGroupQueryHandler,
            IRepository<Group> groupsRepository)
        {
            _userGroupQueryHandler =
                userGroupQueryHandler ?? throw new ArgumentNullException(nameof(userGroupQueryHandler));
            _groupsRepository = groupsRepository ?? throw new ArgumentNullException(nameof(groupsRepository));
        }

        public async Task<UpdateGroupDetailsCommandResult> Handle(
            UpdateGroupDetailsCommand request,
            CancellationToken cancellationToken)
        {
            var group = await _userGroupQueryHandler.HandleAsync(
                new UserGroupQuery(request.UserId, request.GroupId),
                cancellationToken);

            if (group is null)
            {
                return null;
            }

            if (!group.GroupUsers.Any(gu => gu.User.Id == request.UserId && gu.Role == GroupUserRole.Admin))
            {
                // TODO: use a better error strategy
                throw new UnauthorizedAccessException("User is not authorized to edit this group");
            }

            group.Name = request.Name;
            group.RowVersion = uint.Parse(request.RowVersion);
            await _groupsRepository.UpdateAsync(group, cancellationToken);

            return new UpdateGroupDetailsCommandResult(
                group.Id,
                group.Name,
                group.RowVersion.ToString());
        }
    }
}
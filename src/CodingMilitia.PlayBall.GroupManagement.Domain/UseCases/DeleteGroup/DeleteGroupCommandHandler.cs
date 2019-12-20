using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CodingMilitia.PlayBall.GroupManagement.Domain.Data;
using CodingMilitia.PlayBall.GroupManagement.Domain.Data.Queries;
using CodingMilitia.PlayBall.GroupManagement.Domain.Entities;
using MediatR;

namespace CodingMilitia.PlayBall.GroupManagement.Domain.UseCases.DeleteGroup
{
    public class DeleteGroupCommandHandler : IRequestHandler<DeleteGroupCommand, Unit>
    {
        private readonly IQueryHandler<UserGroupQuery, Group> _userGroupQueryHandler;
        private readonly IRepository<Group> _groupsRepository;

        public DeleteGroupCommandHandler(
            IQueryHandler<UserGroupQuery, Group> userGroupQueryHandler,
            IRepository<Group> groupsRepository)
        {
            _userGroupQueryHandler =
                userGroupQueryHandler ?? throw new ArgumentNullException(nameof(userGroupQueryHandler));
            _groupsRepository = groupsRepository ?? throw new ArgumentNullException(nameof(groupsRepository));
        }

        public async Task<Unit> Handle(DeleteGroupCommand request, CancellationToken cancellationToken)
        {
            var group = await _userGroupQueryHandler.HandleAsync(
                new UserGroupQuery(request.UserId, request.GroupId),
                cancellationToken);

            if (group is object
                && group.GroupUsers.Any(gu => gu.User.Id == request.UserId && gu.Role == GroupUserRole.Admin))
            {
                await _groupsRepository.DeleteAsync(group, cancellationToken);
            }

            return Unit.Value;
        }
    }
}
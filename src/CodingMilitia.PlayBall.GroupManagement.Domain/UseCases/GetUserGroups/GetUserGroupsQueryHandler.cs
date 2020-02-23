using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CodingMilitia.PlayBall.GroupManagement.Domain.Data;
using CodingMilitia.PlayBall.GroupManagement.Domain.Data.Queries;
using CodingMilitia.PlayBall.GroupManagement.Domain.Entities;
using CodingMilitia.PlayBall.GroupManagement.Domain.Shared;
using MediatR;

namespace CodingMilitia.PlayBall.GroupManagement.Domain.UseCases.GetUserGroups
{
    public sealed class GetUserGroupsQueryHandler : IRequestHandler<GetUserGroupsQuery, GetUserGroupsQueryResult>
    {
        private readonly IQueryHandler<UserGroupsQuery, IReadOnlyCollection<Group>> _userGroupsQueryHandler;

        public GetUserGroupsQueryHandler(
            IQueryHandler<UserGroupsQuery, IReadOnlyCollection<Group>> userGroupsQueryHandler)
        {
            _userGroupsQueryHandler = Ensure.NotNull(userGroupsQueryHandler, nameof(userGroupsQueryHandler));
        }

        public async Task<GetUserGroupsQueryResult> Handle(
            GetUserGroupsQuery request,
            CancellationToken cancellationToken)
        {
            var groups = await _userGroupsQueryHandler.HandleAsync(
                new UserGroupsQuery(request.UserId),
                cancellationToken);

            return new GetUserGroupsQueryResult(
                groups.MapAsReadOnly(g =>
                    new GetUserGroupsQueryResult.Group(
                        g.Id,
                        g.Name,
                        g.RowVersion.ToString(),
                        new GetUserGroupsQueryResult.User(g.Creator.Id, g.Creator.Name))));
        }
    }
}
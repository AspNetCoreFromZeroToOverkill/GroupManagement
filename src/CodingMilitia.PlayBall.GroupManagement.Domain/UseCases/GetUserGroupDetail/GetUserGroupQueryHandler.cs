using System;
using System.Threading;
using System.Threading.Tasks;
using CodingMilitia.PlayBall.GroupManagement.Domain.Data;
using CodingMilitia.PlayBall.GroupManagement.Domain.Data.Queries;
using CodingMilitia.PlayBall.GroupManagement.Domain.Entities;
using CodingMilitia.PlayBall.GroupManagement.Domain.Shared;
using MediatR;

namespace CodingMilitia.PlayBall.GroupManagement.Domain.UseCases.GetUserGroupDetail
{
    public sealed class GetUserGroupQueryHandler : IRequestHandler<GetUserGroupQuery, Optional<GetUserGroupQueryResult>>
    {
        private readonly IQueryHandler<UserGroupQuery, Optional<Group>> _userGroupQueryHandler;


        public GetUserGroupQueryHandler(IQueryHandler<UserGroupQuery, Optional<Group>> userGroupQueryHandler)
        {
            _userGroupQueryHandler =
                userGroupQueryHandler ?? throw new ArgumentNullException(nameof(userGroupQueryHandler));
        }


        public async Task<Optional<GetUserGroupQueryResult>> Handle(
            GetUserGroupQuery request,
            CancellationToken cancellationToken)
        {
            var maybeGroup = await _userGroupQueryHandler.HandleAsync(
                new UserGroupQuery(request.UserId, request.GroupId),
                cancellationToken);

            return maybeGroup.Map(
                group => new GetUserGroupQueryResult(
                    group.Id,
                    group.Name,
                    group.RowVersion.ToString(),
                    new GetUserGroupQueryResult.User(group.Creator.Id, group.Creator.Name)));
        }
    }
}
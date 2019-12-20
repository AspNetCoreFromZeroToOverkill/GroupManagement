using System;
using System.Threading;
using System.Threading.Tasks;
using CodingMilitia.PlayBall.GroupManagement.Domain.Data;
using CodingMilitia.PlayBall.GroupManagement.Domain.Data.Queries;
using CodingMilitia.PlayBall.GroupManagement.Domain.Entities;
using MediatR;

namespace CodingMilitia.PlayBall.GroupManagement.Domain.UseCases.GetUserGroupDetail
{
    public sealed class GetUserGroupQueryHandler : IRequestHandler<GetUserGroupQuery, GetUserGroupQueryResult>
    {
        private readonly IQueryHandler<UserGroupQuery, Group> _userGroupQueryHandler;


        public GetUserGroupQueryHandler(IQueryHandler<UserGroupQuery, Group> userGroupQueryHandler)
        {
            _userGroupQueryHandler =
                userGroupQueryHandler ?? throw new ArgumentNullException(nameof(userGroupQueryHandler));
        }


        public async Task<GetUserGroupQueryResult> Handle(
            GetUserGroupQuery request,
            CancellationToken cancellationToken)
        {
            var group = await _userGroupQueryHandler.HandleAsync(
                new UserGroupQuery(request.UserId, request.GroupId),
                cancellationToken);

            if (group is null)
            {
                return null;
            }
            
            return new GetUserGroupQueryResult(
                group.Id,
                group.Name,
                group.RowVersion.ToString(),
                new GetUserGroupQueryResult.User(group.Creator.Id, group.Creator.Name));
        }
    }
}
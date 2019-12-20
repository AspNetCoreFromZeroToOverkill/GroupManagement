using MediatR;

namespace CodingMilitia.PlayBall.GroupManagement.Domain.UseCases.GetUserGroups
{
    public sealed class GetUserGroupsQuery : IRequest<GetUserGroupsQueryResult>
    {
        public GetUserGroupsQuery(string userId)
        {
            UserId = userId;
        }
        
        public string UserId { get; }
    }
}
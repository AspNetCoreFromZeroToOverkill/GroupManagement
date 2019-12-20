using MediatR;

namespace CodingMilitia.PlayBall.GroupManagement.Domain.UseCases.GetUserGroupDetail
{
    public sealed class GetUserGroupQuery : IRequest<GetUserGroupQueryResult>
    {
        public GetUserGroupQuery(string userId, long groupId)
        {
            UserId = userId;
            GroupId = groupId;
        }

        public string UserId { get; }

        public long GroupId { get; }
    }
}
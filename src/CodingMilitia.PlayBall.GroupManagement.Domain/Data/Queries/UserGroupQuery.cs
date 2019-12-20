using CodingMilitia.PlayBall.GroupManagement.Domain.Entities;

namespace CodingMilitia.PlayBall.GroupManagement.Domain.Data.Queries
{
    public class UserGroupQuery : IQuery<Group>
    {
        public UserGroupQuery(string userId, long groupId)
        {
            UserId = userId;
            GroupId = groupId;
        }

        public string UserId { get; }

        public long GroupId { get; }
    }
}
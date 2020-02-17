using CodingMilitia.PlayBall.GroupManagement.Domain.Entities;
using CodingMilitia.PlayBall.GroupManagement.Domain.Shared;

namespace CodingMilitia.PlayBall.GroupManagement.Domain.Data.Queries
{
    public class UserGroupQuery : IQuery<Optional<Group>>
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
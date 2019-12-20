using System.Collections.Generic;
using CodingMilitia.PlayBall.GroupManagement.Domain.Entities;

namespace CodingMilitia.PlayBall.GroupManagement.Domain.Data.Queries
{
    public class UserGroupsQuery: IQuery<IReadOnlyCollection<Group>>
    {
        public UserGroupsQuery(string userId)
        {
            UserId = userId;
        }

        public string UserId { get; }
    }
}
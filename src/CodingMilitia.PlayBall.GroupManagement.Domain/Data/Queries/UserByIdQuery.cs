using CodingMilitia.PlayBall.GroupManagement.Domain.Entities;

namespace CodingMilitia.PlayBall.GroupManagement.Domain.Data.Queries
{
    public class UserByIdQuery : IQuery<User>
    {
        public UserByIdQuery(string userId)
        {
            UserId = userId;
        }

        public string UserId { get; }
    }
}
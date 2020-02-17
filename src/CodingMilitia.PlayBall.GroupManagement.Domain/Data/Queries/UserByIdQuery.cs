using CodingMilitia.PlayBall.GroupManagement.Domain.Entities;
using CodingMilitia.PlayBall.GroupManagement.Domain.Shared;

namespace CodingMilitia.PlayBall.GroupManagement.Domain.Data.Queries
{
    public class UserByIdQuery : IQuery<Optional<User>>
    {
        public UserByIdQuery(string userId)
        {
            UserId = userId;
        }

        public string UserId { get; }
    }
}
using System.Collections.Generic;

namespace CodingMilitia.PlayBall.GroupManagement.Domain.Entities
{
    public class User
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public ICollection<GroupUser> UserGroups { get; set; } = new List<GroupUser>();
    }
}
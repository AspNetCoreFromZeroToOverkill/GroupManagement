using System.Collections.Generic;
using CodingMilitia.PlayBall.GroupManagement.Domain.Data;

namespace CodingMilitia.PlayBall.GroupManagement.Domain.Entities
{
    public class Group : IVersionedEntity
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public uint RowVersion { get; set; }
        public User Creator { get; set; }
        public ICollection<GroupUser> GroupUsers { get; set; } = new List<GroupUser>();
    }
}
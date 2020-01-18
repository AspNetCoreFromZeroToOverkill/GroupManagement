using System;
using System.Collections.Generic;

namespace CodingMilitia.PlayBall.GroupManagement.Domain.Entities
{
    public class User
    {
        private readonly List<GroupUser> _userGroups = new List<GroupUser>();

        public User(string id, string name)
        {
            Id = !string.IsNullOrWhiteSpace(id) ? id : throw new ArgumentNullException(nameof(id));
            Name = !string.IsNullOrWhiteSpace(name) ? name : throw new ArgumentNullException(nameof(name));
        }
        
        public string Id { get; private set; }
        public string Name { get; private set; }
        public IReadOnlyCollection<GroupUser> UserGroups => _userGroups.AsReadOnly();
    }
}
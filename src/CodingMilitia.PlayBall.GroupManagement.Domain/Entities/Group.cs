using System;
using System.Collections.Generic;
using System.Linq;
using CodingMilitia.PlayBall.GroupManagement.Domain.Data;

namespace CodingMilitia.PlayBall.GroupManagement.Domain.Entities
{
    public class Group : IVersionedEntity
    {
        private readonly List<GroupUser> _groupUsers = new List<GroupUser>();

        public Group(string name, User creator)
        {
            Name = !string.IsNullOrWhiteSpace(name) ? name : throw new ArgumentNullException(nameof(name));
            Creator = creator ?? throw new ArgumentNullException(nameof(creator));
            _groupUsers.Add(GroupUser.NewAdministrator(Id, creator.Id));
        }

        private Group()
        {
        }

        public long Id { get; private set; }
        public string Name { get; private set; }
        public uint RowVersion { get; set; } //TODO: make setter private
        public User Creator { get; private set; }
        public IReadOnlyCollection<GroupUser> GroupUsers => _groupUsers.AsReadOnly();

        public void Rename(User editingUser, string newName)
        {
            ThrowIfNotAdmin(editingUser.Id);

            Name = !string.IsNullOrWhiteSpace(newName) ? newName : throw new ArgumentNullException(nameof(newName));
        }

        public bool IsAdmin(string userId)
            => GroupUsers.Any(gu => gu.UserId == userId && gu.Role == GroupUserRole.Admin);

        // TODO: temporary we'll get rid of all these exceptions eventually
        private void ThrowIfNotAdmin(string userId)
        {
            if (!IsAdmin(userId))
            {
                // TODO: use a better error strategy
                throw new UnauthorizedAccessException("User is not authorized to edit this group");
            }
        }
    }
}
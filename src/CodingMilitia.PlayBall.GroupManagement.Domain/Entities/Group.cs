using System;
using System.Collections.Generic;
using System.Linq;
using CodingMilitia.PlayBall.GroupManagement.Domain.Data;
using CodingMilitia.PlayBall.GroupManagement.Domain.Shared;
using MediatR;

namespace CodingMilitia.PlayBall.GroupManagement.Domain.Entities
{
    public class Group : IVersionedEntity<uint>
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
        public uint RowVersion { get; private set; }
        public User Creator { get; private set; }
        public IReadOnlyCollection<GroupUser> GroupUsers => _groupUsers.AsReadOnly();

        public Either<Error, Unit> Rename(User editingUser, string newName)
        {
            if (!IsAdmin(editingUser.Id))
            {
                return Result.Unauthorized<Unit>("User is not authorized to edit this group");
            }
            
            if (string.IsNullOrWhiteSpace(newName))
            {
                return Result.Invalid<Unit>("The group's name cannot be empty.");
            }

            Name = newName;

            return Result.Success(Unit.Value);
        }

        public bool IsAdmin(string userId)
            => GroupUsers.Any(gu => gu.UserId == userId && gu.Role == GroupUserRole.Admin);
    }
}
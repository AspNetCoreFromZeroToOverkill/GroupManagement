using System;

namespace CodingMilitia.PlayBall.GroupManagement.Domain.Entities
{
    public class GroupUser
    {
        public GroupUser(long groupId, string userId, GroupUserRole role)
        {
            GroupId = groupId;
            UserId = !string.IsNullOrWhiteSpace(userId) ? userId : throw new ArgumentNullException(nameof(userId));
            Role = role;
        }

        public long GroupId { get; private set; }
        public string UserId { get; private set; }
        public GroupUserRole Role { get; private set; }

        public static GroupUser NewAdministrator(long groupId, string userId)
            => new GroupUser(groupId, userId, GroupUserRole.Admin);

        public static GroupUser NewParticipant(long groupId, string userId)
            => new GroupUser(groupId, userId, GroupUserRole.Participant);
    }
}
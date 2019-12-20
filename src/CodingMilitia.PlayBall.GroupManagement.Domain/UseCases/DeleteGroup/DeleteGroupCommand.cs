using MediatR;

namespace CodingMilitia.PlayBall.GroupManagement.Domain.UseCases.DeleteGroup
{
    public sealed class DeleteGroupCommand : IRequest<Unit>
    {
        public DeleteGroupCommand(string userId, long groupId)
        {
            UserId = userId;
            GroupId = groupId;
        }

        public string UserId { get; }

        public long GroupId { get; }
    }
}
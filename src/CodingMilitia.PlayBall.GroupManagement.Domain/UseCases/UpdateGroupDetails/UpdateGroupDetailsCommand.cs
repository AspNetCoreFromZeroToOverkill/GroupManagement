using MediatR;

namespace CodingMilitia.PlayBall.GroupManagement.Domain.UseCases.UpdateGroupDetails
{
    public sealed class UpdateGroupDetailsCommand : IRequest<UpdateGroupDetailsCommandResult>
    {
        public UpdateGroupDetailsCommand(string userId, long groupId, string name, string rowVersion)
        {
            UserId = userId;
            GroupId = groupId;
            Name = name;
            RowVersion = rowVersion;
        }
        
        public string UserId { get; }
        public long GroupId { get; }
        public string Name { get; }
        public string RowVersion { get; }
    }
}
using MediatR;

namespace CodingMilitia.PlayBall.GroupManagement.Domain.UseCases.CreateGroup
{
    public sealed class CreateGroupCommand : IRequest<CreateGroupCommandResult>
    {
        public CreateGroupCommand(string userId, string name)
        {
            UserId = userId;
            Name = name;
        }

        public string UserId { get; }
        public string Name { get; }
    }
}
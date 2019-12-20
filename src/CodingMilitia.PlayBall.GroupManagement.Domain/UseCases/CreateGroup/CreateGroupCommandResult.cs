namespace CodingMilitia.PlayBall.GroupManagement.Domain.UseCases.CreateGroup
{
    public sealed class CreateGroupCommandResult
    {
        public CreateGroupCommandResult(long id, string name, string rowVersion, User creator)
        {
            Id = id;
            Name = name;
            RowVersion = rowVersion;
            Creator = creator;
        }
        
        public long Id { get; }
        public string Name { get; }
        public string RowVersion { get; }
        public User Creator { get; }

        public class User
        {
            public User(string id, string name)
            {
                Id = id;
                Name = name;
            }

            public string Id { get; }
            public string Name { get; }
        }
    }
}
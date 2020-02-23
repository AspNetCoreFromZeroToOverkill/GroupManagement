namespace CodingMilitia.PlayBall.GroupManagement.Domain.UseCases.UpdateGroupDetails
{
    public sealed class UpdateGroupDetailsCommandResult
    {
        public UpdateGroupDetailsCommandResult(long id, string name, string rowVersion)
        {
            Id = id;
            Name = name;
            RowVersion = rowVersion;
        }

        public long Id { get; }
        public string Name { get; }
        public string RowVersion { get; }
    }
}
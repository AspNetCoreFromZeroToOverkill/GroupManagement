namespace CodingMilitia.PlayBall.GroupManagement.Domain.Data
{
    public interface IVersionedEntity
    {
        uint RowVersion { get; }
    }
}
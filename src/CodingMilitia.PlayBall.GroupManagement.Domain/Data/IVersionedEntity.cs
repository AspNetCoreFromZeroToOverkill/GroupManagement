namespace CodingMilitia.PlayBall.GroupManagement.Domain.Data
{
    public interface IVersionedEntity<TVersion>
    {
        TVersion RowVersion { get; }
    }
}
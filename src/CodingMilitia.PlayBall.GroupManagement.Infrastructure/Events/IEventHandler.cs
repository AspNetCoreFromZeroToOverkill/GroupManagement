using System.Threading.Tasks;

namespace CodingMilitia.PlayBall.GroupManagement.Infrastructure.Events
{
    public interface IEventHandler<in TEvent>
    {
        Task HandleAsync(TEvent @event);
    }
}
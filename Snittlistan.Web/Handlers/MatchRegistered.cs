using Snittlistan.Web.Areas.V2.Domain.Match;
using Snittlistan.Web.Tasks;

namespace Snittlistan.Web.Handlers
{
    public class MatchRegistered : HandleBase<MatchRegisteredEvent>
    {
        public override void Handle(MatchRegisteredEvent @event)
        {
            var task = new MatchRegisteredTask(@event.RosterId, @event.Score, @event.OpponentScore);
            SendTask(task);
        }
    }
}
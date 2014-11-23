using Snittlistan.Web.Areas.V2.Domain;
using Snittlistan.Web.Areas.V2.Domain.Match;
using Snittlistan.Web.Tasks;

namespace Snittlistan.Web.Handlers
{
    public class MatchRegistered : HandleBase<MatchRegisteredEvent>
    {
        public override void Handle(MatchRegisteredEvent @event)
        {
            var roster = DocumentSession.Load<Roster>(@event.RosterId);
            var subject = string.Format("{0} mot {1}: {2} - {3}", roster.Team, roster.Opponent, @event.Score, @event.OpponentScore);

            SendTask(new MatchRegisteredTask(subject, roster.Team, roster.Opponent, @event.Score, @event.OpponentScore));
        }
    }
}
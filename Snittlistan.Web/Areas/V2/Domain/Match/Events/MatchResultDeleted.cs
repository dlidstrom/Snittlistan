using EventStoreLite;

namespace Snittlistan.Web.Areas.V2.Domain.Match.Events
{
    public class MatchResultDeleted : Event
    {
        public MatchResultDeleted(string rosterId, int bitsMatchId)
        {
            BitsMatchId = bitsMatchId;
            RosterId = rosterId;
        }

        public string RosterId { get; private set; }

        public int BitsMatchId { get; private set; }
    }
}
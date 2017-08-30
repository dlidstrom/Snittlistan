using EventStoreLite;

namespace Snittlistan.Web.Areas.V2.Domain.Match.Events
{
    public class MatchCommentaryEvent : Event
    {
        public MatchCommentaryEvent(int bitsMatchId, string matchCommentary)
        {
            BitsMatchId = bitsMatchId;
            MatchCommentary = matchCommentary;
        }

        public int BitsMatchId { get; private set; }

        public string MatchCommentary { get; private set; }
    }
}
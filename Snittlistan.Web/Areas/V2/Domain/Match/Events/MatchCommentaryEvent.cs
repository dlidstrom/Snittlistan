using EventStoreLite;

namespace Snittlistan.Web.Areas.V2.Domain.Match.Events
{
    public class MatchCommentaryEvent : Event
    {
        public MatchCommentaryEvent(
            int bitsMatchId,
            string rosterId,
            string summaryText,
            string[] bodyText)
        {
            BitsMatchId = bitsMatchId;
            RosterId = rosterId;
            SummaryText = summaryText;
            BodyText = bodyText;
        }

        public int BitsMatchId { get; private set; }

        public string RosterId { get; private set; }

        public string SummaryText { get; private set; }

        public string[] BodyText { get; private set; }
    }
}
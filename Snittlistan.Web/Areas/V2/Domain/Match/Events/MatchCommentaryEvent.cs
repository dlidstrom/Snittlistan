
using EventStoreLite;

#nullable enable

namespace Snittlistan.Web.Areas.V2.Domain.Match.Events;
public class MatchCommentaryEvent : Event
{
    public MatchCommentaryEvent(
        int bitsMatchId,
        string rosterId,
        string summaryText,
        string summaryHtml,
        string[] bodyText)
    {
        BitsMatchId = bitsMatchId;
        RosterId = rosterId;
        SummaryText = summaryText;
        SummaryHtml = summaryHtml;
        BodyText = bodyText;
    }

    public int BitsMatchId { get; }

    public string RosterId { get; }

    public string SummaryText { get; }

    public string SummaryHtml { get; }

    public string[] BodyText { get; }
}

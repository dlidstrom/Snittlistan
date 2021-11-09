#nullable enable

namespace Snittlistan.Web.Areas.V2.Domain.Match.Events
{
    using System;
    using EventStoreLite;

    public class Serie4Registered : Event
    {
        public Serie4Registered(MatchSerie4 matchSerie, int bitsMatchId, string rosterId)
        {
            MatchSerie = matchSerie ?? throw new ArgumentNullException(nameof(matchSerie));
            BitsMatchId = bitsMatchId;
            RosterId = rosterId;
        }

        public MatchSerie4 MatchSerie { get; }

        public int BitsMatchId { get; }

        public string RosterId { get; }
    }
}

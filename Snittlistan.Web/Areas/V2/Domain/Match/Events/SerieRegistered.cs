using System;
using EventStoreLite;

namespace Snittlistan.Web.Areas.V2.Domain.Match.Events
{
    public class SerieRegistered : Event
    {
        public SerieRegistered(MatchSerie matchSerie, int bitsMatchId, string rosterId)
        {
            MatchSerie = matchSerie ?? throw new ArgumentNullException(nameof(matchSerie));
            BitsMatchId = bitsMatchId;
            RosterId = rosterId ?? throw new ArgumentNullException(nameof(rosterId));
        }

        public MatchSerie MatchSerie { get; private set; }

        public int BitsMatchId { get; private set; }

        public string RosterId { get; private set; }
    }
}
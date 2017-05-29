using System;
using EventStoreLite;

namespace Snittlistan.Web.Areas.V2.Domain.Match.Events
{
    public class SerieRegistered : Event
    {
        public SerieRegistered(MatchSerie matchSerie, int bitsMatchId, string rosterId)
        {
            if (matchSerie == null) throw new ArgumentNullException("matchSerie");
            if (rosterId == null) throw new ArgumentNullException("rosterId");
            MatchSerie = matchSerie;
            BitsMatchId = bitsMatchId;
            RosterId = rosterId;
        }

        public MatchSerie MatchSerie { get; private set; }

        public int BitsMatchId { get; private set; }

        public string RosterId { get; private set; }
    }
}
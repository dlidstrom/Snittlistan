using System;
using EventStoreLite;

namespace Snittlistan.Web.Areas.V2.Domain.Match.Events
{
    public class Serie4Registered : Event
    {
        public Serie4Registered(MatchSerie4 matchSerie, int bitsMatchId)
        {
            MatchSerie = matchSerie ?? throw new ArgumentNullException(nameof(matchSerie));
            BitsMatchId = bitsMatchId;
        }

        public MatchSerie4 MatchSerie { get; private set; }

        public int BitsMatchId { get; private set; }
    }
}
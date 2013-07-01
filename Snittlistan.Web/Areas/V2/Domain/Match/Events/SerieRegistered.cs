using System;
using EventStoreLite;

namespace Snittlistan.Web.Areas.V2.Domain.Match.Events
{
    public class SerieRegistered : Event
    {
        public SerieRegistered(MatchSerie matchSerie, int bitsMatchId)
        {
            if (matchSerie == null) throw new ArgumentNullException("matchSerie");
            MatchSerie = matchSerie;
            BitsMatchId = bitsMatchId;
        }

        public MatchSerie MatchSerie { get; private set; }

        public int BitsMatchId { get; private set; }
    }
}
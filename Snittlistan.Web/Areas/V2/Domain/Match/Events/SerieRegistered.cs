using System;
using EventStoreLite;

namespace Snittlistan.Web.Areas.V2.Domain.Match.Events
{
    public class SerieRegistered : Event
    {
        public MatchSerie MatchSerie { get; set; }

        public SerieRegistered(MatchSerie matchSerie)
        {
            if (matchSerie == null) throw new ArgumentNullException("matchSerie");
            this.MatchSerie = matchSerie;
        }
    }
}
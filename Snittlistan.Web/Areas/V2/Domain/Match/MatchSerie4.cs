using System;
using System.Collections.Generic;
using System.Linq;

namespace Snittlistan.Web.Areas.V2.Domain.Match
{
    public class MatchSerie4
    {
        public MatchSerie4(IReadOnlyList<MatchGame4> games)
        {
            if (games == null) throw new ArgumentNullException("games");
            if (games.Count != 4) throw new ArgumentException("games");

            var players = new HashSet<string>(games.AsEnumerable().Select(x => x.Player));

            if (players.Count != 4)
                throw new MatchException("Serie must have 4 different players");

            Game1 = games[0];
            Game2 = games[1];
            Game3 = games[2];
            Game4 = games[3];
        }

        // necessary for json.net
        private MatchSerie4()
        {
        }

        public MatchGame4 Game1 { get; private set; }

        public MatchGame4 Game2 { get; private set; }

        public MatchGame4 Game3 { get; private set; }

        public MatchGame4 Game4 { get; private set; }
    }
}
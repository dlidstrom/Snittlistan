using System;
using System.Collections.Generic;
using Raven.Imports.Newtonsoft.Json;

namespace Snittlistan.Web.Areas.V2.Domain.Match
{
    public class MatchSerie
    {
        private MatchSerie()
        {
        }

        public MatchSerie(IReadOnlyList<MatchTable> tables)
        {
            if (tables == null) throw new ArgumentNullException("tables");
            if (tables.Count != 4) throw new ArgumentException("tables");

            var players = new HashSet<string>();
            foreach (var matchTable in tables)
            {
                var p1 = matchTable.Game1.Player;
                var p2 = matchTable.Game2.Player;
                players.Add(p1);
                players.Add(p2);
            }

            if (players.Count != 8)
                throw new MatchException("Serie must have 8 different players");

            this.Table1 = tables[0];
            this.Table2 = tables[1];
            this.Table3 = tables[2];
            this.Table4 = tables[3];
        }

        public MatchTable Table1 { get; private set; }

        public MatchTable Table2 { get; private set; }

        public MatchTable Table3 { get; private set; }

        public MatchTable Table4 { get; private set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;

namespace Snittlistan.Web.Areas.V2.Domain.Match
{
    public class MatchSerie
    {
        // necessary for json.net
        private MatchSerie()
        {
        }

        public MatchSerie(int serieNumber, IReadOnlyList<MatchTable> tables)
        {
            if (tables == null) throw new ArgumentNullException(nameof(tables));
            if (tables.Count != 4) throw new ArgumentException("tables");
            if (tables[0].TableNumber != 1
                || tables[1].TableNumber != 2
                || tables[2].TableNumber != 3
                || tables[3].TableNumber != 4)
            {
                throw new ArgumentException("Invalid table number", nameof(tables));
            }

            SerieNumber = serieNumber;
            var players = new HashSet<string>();
            foreach (var matchTable in tables)
            {
                var p1 = matchTable.Game1.Player;
                var p2 = matchTable.Game2.Player;
                players.Add(p1);
                players.Add(p2);
            }

            if (players.Count != 7 && players.Count != 8)
            {
                throw new MatchException($"Serie {serieNumber} must have 7 or 8 different players (detected {players.Count} players)");
            }

            Table1 = tables[0];
            Table2 = tables[1];
            Table3 = tables[2];
            Table4 = tables[3];
        }

        public int SerieNumber { get; private set; }

        public MatchTable Table1 { get; private set; }

        public MatchTable Table2 { get; private set; }

        public MatchTable Table3 { get; private set; }

        public MatchTable Table4 { get; private set; }

        public int TeamTotal
        {
            get
            {
                return new[]
                {
                    Table1.Game1.Pins,
                    Table1.Game2.Pins,
                    Table2.Game1.Pins,
                    Table2.Game2.Pins,
                    Table3.Game1.Pins,
                    Table3.Game2.Pins,
                    Table4.Game1.Pins,
                    Table4.Game2.Pins,
                }.Sum();
            }
        }
    }
}
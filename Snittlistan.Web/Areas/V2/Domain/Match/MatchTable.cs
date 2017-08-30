using System;
using System.Diagnostics;

namespace Snittlistan.Web.Areas.V2.Domain.Match
{
    [DebuggerDisplay("{Game1.Player} {Game2.Player} {Score}")]
    public class MatchTable
    {
        public MatchTable(int tableNumber, MatchGame game1, MatchGame game2, int score)
        {
            if (game1 == null) throw new ArgumentNullException("game1");
            if (game2 == null) throw new ArgumentNullException("game2");
            if (score != 0 && score != 1) throw new ArgumentOutOfRangeException("score", score, "Score out of range");
            if (game1.Player == game2.Player)
                throw new MatchException("Table must have different players");
            TableNumber = tableNumber;
            Score = score;
            Game1 = game1;
            Game2 = game2;
        }

        // @TODO From constructor
        public int TableNumber { get; set; }

        public MatchGame Game1 { get; private set; }

        public MatchGame Game2 { get; private set; }

        public int Score { get; private set; }
    }
}
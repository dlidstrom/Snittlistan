using System;
using System.Diagnostics;

namespace Snittlistan.Web.Areas.V2.Domain.Match
{
    [DebuggerDisplay("{Player} Score={Score} Pins={Pins}")]
    public class MatchGame4
    {
        public MatchGame4(string player, int score, int pins)
        {
            if (player == null) throw new ArgumentNullException("player");
            if (pins < 0 || pins > 300)
                throw new ArgumentException("Pins out of range", "pins");
            Player = player;
            Score = score;
            Pins = pins;
        }

        public string Player { get; private set; }

        public int Score { get; private set; }

        public int Pins { get; private set; }
    }
}
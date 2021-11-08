namespace Snittlistan.Web.Areas.V2.Domain.Match
{
    using System;
    using System.Diagnostics;

    [DebuggerDisplay("{Player} Pins={Pins} X={Strikes} Spares={Spares}")]
    public class MatchGame
    {
        public MatchGame(string player, int pins, int strikes, int spares)
        {
            Player = player ?? throw new ArgumentNullException(nameof(player));
            if (pins < 0 || pins > 300)
            {
                throw new ArgumentException("Pins out of range", nameof(pins));
            }

            if (strikes < 0 || strikes > 12)
            {
                throw new ArgumentException("Strikes out of range", nameof(strikes));
            }

            if (spares < 0 || spares > 10 || spares > Math.Max(0, 12 - strikes))
            {
                throw new ArgumentException("Spares out of range", nameof(spares));
            }

            Pins = pins;
            Strikes = strikes;
            Spares = spares;
        }

        public string Player { get; }

        public int Pins { get; }

        public int Strikes { get; }

        public int Spares { get; }
    }
}
using System.Collections.Generic;
using System.Globalization;

namespace Snittlistan.Web.Areas.V2.ReadModels
{
    public class PlayerScore
    {
        public PlayerScore(string name, string team)
        {
            Team = team;
            Name = name;
        }

        public string Name { get; private set; }

        public int Pins { get; set; }

        public int Series { get; set; }

        public string PinsAndSeries
        {
            get
            {
                return Series != 4
                    ? string.Format("{0} ({1})", Pins, Series)
                    : Pins.ToString(CultureInfo.InvariantCulture);
            }
        }

        public string Team { get; private set; }

        public class Comparer : IComparer<PlayerScore>
        {
            public int Compare(PlayerScore x, PlayerScore y)
            {
                return string.CompareOrdinal(x.Name, y.Name);
            }
        }
    }
}
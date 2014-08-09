using System.Collections.Generic;
using System.Globalization;

namespace Snittlistan.Web.Areas.V2.ReadModels
{
    public class PlayerScore
    {
        public PlayerScore(string playerId, string name, string team, string teamLevel)
        {
            Medals = new List<AwardedMedalReadModel>();
            PlayerId = playerId;
            Team = team;
            TeamLevel = teamLevel;
            Name = name;
        }

        public string PlayerId { get; private set; }

        public string Name { get; private set; }

        public int Score { get; set; }

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

        public string TeamLevel { get; private set; }

        public List<AwardedMedalReadModel> Medals { get; set; }

        public void AddMedal(AwardedMedalReadModel awardedMedal)
        {
            Medals.Add(awardedMedal);
        }

        public void ClearMedals()
        {
            Medals.Clear();
        }

        public class Comparer : IComparer<PlayerScore>
        {
            public int Compare(PlayerScore x, PlayerScore y)
            {
                return string.CompareOrdinal(x.Name, y.Name);
            }
        }
    }
}
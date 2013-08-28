using System.Collections.Generic;
using System.Globalization;
using EventStoreLite;
using Snittlistan.Web.Areas.V2.Domain;

namespace Snittlistan.Web.Areas.V2.ReadModels
{
    public class TeamOfWeek : IReadModel
    {
        public TeamOfWeek(int bitsMatchId, int season, int turn, string team)
        {
            PlayerScores = new Dictionary<string, PlayerScore>();
            Id = IdFromBitsMatchId(bitsMatchId);
            Season = season;
            Turn = turn;
            Team = team;
        }

        public string Id { get; private set; }

        public Dictionary<string, PlayerScore> PlayerScores { get; private set; }

        public int Season { get; private set; }

        public string Team { get; private set; }

        public int Turn { get; private set; }

        public static string IdFromBitsMatchId(int bitsMatchId)
        {
            return "TeamOfWeek-" + bitsMatchId;
        }

        public void AddResultForPlayer(Player player, int pins)
        {
            if (PlayerScores.ContainsKey(player.Id) == false)
            {
                PlayerScores.Add(player.Id, new PlayerScore(player.Name, Team));
            }

            var playerScore = PlayerScores[player.Id];
            playerScore.Pins += pins;
            playerScore.Series++;
        }

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
        }
    }
}
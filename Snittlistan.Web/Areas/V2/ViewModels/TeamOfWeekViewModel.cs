using System.Collections.Generic;
using System.Linq;
using Snittlistan.Web.Areas.V2.ReadModels;

namespace Snittlistan.Web.Areas.V2.ViewModels
{
    public class TeamOfWeekViewModel
    {
        public TeamOfWeekViewModel(
            int season,
            TeamOfWeek[] teamOfWeek)
        {
            Leaders = new TeamOfWeekLeadersViewModel(teamOfWeek);
            Season = season;
            Weeks = new List<Week>();
            foreach (var turn in teamOfWeek.GroupBy(x => x.Turn))
            {
                var q = from t in turn
                        from playerScore in t.PlayerScores
                        group playerScore by new
                        {
                            playerScore.Key,
                            playerScore.Value.PlayerId,
                            playerScore.Value.Name
                        } into g
                        let maxValue = g.OrderByDescending(x => x.Value.Pins).FirstOrDefault()
                        orderby maxValue.Value.Pins descending
                        select new PlayerScore(g.Key.PlayerId, g.Key.Name, maxValue.Value.Team, maxValue.Value.TeamLevel)
                        {
                            Pins = maxValue.Value.Pins,
                            Score = maxValue.Value.Score,
                            Series = maxValue.Value.Series,
                            Medals = g.SelectMany(x => x.Value.Medals).ToList()
                        };
                var week = new Week(turn.Key, q.ToList());
                Weeks.Add(week);
            }
        }

        public int Season { get; private set; }

        public List<Week> Weeks { get; private set; }

        public TeamOfWeekLeadersViewModel Leaders { get; private set; }

        public class Week
        {
            public Week(int turn, List<PlayerScore> playerScores)
            {
                Turn = turn;
                Players = playerScores;
            }

            public List<PlayerScore> Players { get; private set; }

            public int Top8
            {
                get
                {
                    var series = 0;
                    return Players.TakeWhile(x => (series += x.Series) <= 32).Sum(x => x.Pins);
                }
            }

            public int Turn { get; private set; }
        }
    }
}
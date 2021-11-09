#nullable enable

namespace Snittlistan.Web.Areas.V2.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;
    using Snittlistan.Web.Areas.V2.Domain;
    using Snittlistan.Web.Areas.V2.ReadModels;

    public class TeamOfWeekViewModel
    {
        public TeamOfWeekViewModel(
            int season,
            TeamOfWeek[] teamOfWeek,
            Dictionary<string, Roster> rostersDictionary)
        {
            Leaders = new TeamOfWeekLeadersViewModel(teamOfWeek, rostersDictionary);
            Season = season;
            Weeks = new List<Week>();
            foreach (IGrouping<int, TeamOfWeek> turn in teamOfWeek.GroupBy(x => rostersDictionary[x.RosterId].Turn))
            {
                IEnumerable<PlayerScoreViewModel> q =
                    from t in turn
                    from playerScore in t.PlayerScores
                    let item = new { playerScore, t.RosterId }
                    group item by new
                    {
                        PlayerId = item.playerScore.Key,
                        item.playerScore.Value.Name
                    }
                        into g
                    let maxValue = g.OrderByDescending(x => x.playerScore.Value.Pins).FirstOrDefault()
                    orderby maxValue.playerScore.Value.Pins descending
                    select new PlayerScoreViewModel(new PlayerScore(g.Key.PlayerId, g.Key.Name)
                    {
                        Pins = maxValue.playerScore.Value.Pins,
                        Score = maxValue.playerScore.Value.Score,
                        Series = maxValue.playerScore.Value.Series,
                        Medals = g.SelectMany(x => x.playerScore.Value.Medals).ToList()
                    },
                        rostersDictionary[maxValue.RosterId]);
                Week week = new(turn.Key, q.ToArray());
                Weeks.Add(week);
            }
        }

        public int Season { get; private set; }

        public List<Week> Weeks { get; private set; }

        public TeamOfWeekLeadersViewModel Leaders { get; private set; }

        public class Week
        {
            public Week(int turn, PlayerScoreViewModel[] playerScores)
            {
                Turn = turn;
                Players = playerScores;
            }

            public PlayerScoreViewModel[] Players { get; private set; }

            public int Top8
            {
                get
                {
                    int series = 0;
                    return Players.TakeWhile(x => (series += x.Series) <= 32).Sum(x => x.Pins);
                }
            }

            public int Turn { get; private set; }
        }
    }
}

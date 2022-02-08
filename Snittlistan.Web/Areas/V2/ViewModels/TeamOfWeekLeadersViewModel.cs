using Snittlistan.Web.Areas.V2.Domain;
using Snittlistan.Web.Areas.V2.ReadModels;

namespace Snittlistan.Web.Areas.V2.ViewModels;
public class TeamOfWeekLeadersViewModel
{
    public TeamOfWeekLeadersViewModel(TeamOfWeek[] weeks, Dictionary<string, Roster> rostersDictionary)
    {
        var rankByTurn = new Dictionary<int, List<PlayerScore>>();
        foreach (TeamOfWeek week in weeks)
        {
            int turn = rostersDictionary[week.RosterId].Turn;
            if (rankByTurn.TryGetValue(turn, out List<PlayerScore> list) == false)
            {
                rankByTurn[turn] = week.PlayerScores.Values.ToList();
            }
            else
            {
                foreach (PlayerScore value in week.PlayerScores.Values)
                {
                    PlayerScore item = list.SingleOrDefault(x => x.PlayerId == value.PlayerId);
                    if (item == null)
                    {
                        list.Add(value);
                    }
                    else if (item.Pins < value.Pins)
                    {
                        list.Remove(item);
                        list.Add(value);
                    }
                }
            }
        }

        var bestOfBest = new List<Tuple<string, int>>();
        foreach (int turn in rankByTurn.Keys.OrderBy(x => x))
        {
            int current = int.MaxValue;
            int rank = 0;
            int rankstep = 1;
            PlayerScore[] playerScores = rankByTurn[turn].OrderByDescending(x => x.Pins).ToArray();
            foreach (PlayerScore playerScore in playerScores)
            {
                if (playerScore.Pins != current)
                {
                    rank += rankstep;
                    rankstep = 1;
                }
                else
                {
                    rankstep++;
                }

                if (rank > 9)
                {
                    break;
                }

                current = playerScore.Pins;
                bestOfBest.Add(Tuple.Create(playerScore.Name, turn));
            }
        }

        Top9Total = bestOfBest.GroupBy(x => x.Item1)
                              .Select(x => new NameCount(x))
                              .OrderByDescending(x => x.Count)
                              .ThenBy(x => x.Name)
                              .ToArray();
    }

    public NameCount[] Top9Total { get; private set; }

    public class NameCount
    {
        public NameCount(IGrouping<string, Tuple<string, int>> grouping)
        {
            Name = grouping.Key;
            Count = grouping.Count();
            Turns = grouping.Select(x => x.Item2).ToArray();
        }

        public string Name { get; }

        public int Count { get; }

        public int[] Turns { get; }
    }
}

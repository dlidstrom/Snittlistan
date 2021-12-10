using EventStoreLite;
using Snittlistan.Web.Areas.V2.Domain;

#nullable enable

namespace Snittlistan.Web.Areas.V2.ReadModels;
public class TeamOfWeek : IReadModel
{
    public TeamOfWeek(int bitsMatchId, int season, string rosterId)
    {
        PlayerScores = new Dictionary<string, PlayerScore>();
        Id = IdFromBitsMatchId(bitsMatchId, rosterId);
        Season = season;
        RosterId = rosterId;
    }

    public string Id { get; private set; }

    public Dictionary<string, PlayerScore> PlayerScores { get; private set; }

    public int Season { get; private set; }

    // todo make private set
    public string RosterId { get; set; }

    public static string IdFromBitsMatchId(int bitsMatchId, string rosterId)
    {
        if (bitsMatchId != 0)
        {
            return "TeamOfWeek-" + bitsMatchId;
        }

        return $"TeamOfWeek-R{rosterId.Substring(8)}";
    }

    public void AddResultForPlayer(Player player, int score, int pins)
    {
        if (PlayerScores.ContainsKey(player.Id) == false)
        {
            PlayerScores.Add(player.Id, new PlayerScore(player.Id, player.Name));
        }

        PlayerScore playerScore = PlayerScores[player.Id];
        playerScore.Score += score;
        playerScore.Pins += pins;
        playerScore.Series++;
    }

    public void AddMedal(AwardedMedalReadModel awardedMedal)
    {
        if (PlayerScores.ContainsKey(awardedMedal.Player) == false)
        {
            throw new ApplicationException($"No player with id {awardedMedal.Player} found");
        }

        PlayerScores[awardedMedal.Player].AddMedal(awardedMedal);
    }

    public void ClearMedals()
    {
        foreach (string key in PlayerScores.Keys)
        {
            PlayerScores[key].ClearMedals();
        }
    }

    public void Reset()
    {
        PlayerScores = new Dictionary<string, PlayerScore>();
    }
}

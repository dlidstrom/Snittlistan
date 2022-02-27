using Snittlistan.Web.Areas.V2.Domain.Match.Commentary;
using EventStoreLite;
using Newtonsoft.Json;
using Raven.Abstractions;
using Snittlistan.Queue.Messages;
using Snittlistan.Web.Areas.V2.Domain.Match.Events;

#nullable enable

namespace Snittlistan.Web.Areas.V2.Domain.Match;
public class MatchResult4 : AggregateRoot
{
    private readonly Dictionary<string, List<PinsAndScoreResult>> playerPins;
    private HashSet<string>? rosterPlayers;
    private bool medalsAwarded;

    // 1-based
    private int registeredSeries;

    public MatchResult4(Roster roster, int teamScore, int opponentScore, int bitsMatchId)
        : this()
    {
        if (roster == null)
        {
            throw new ArgumentNullException(nameof(roster));
        }

        if (roster.MatchResultId != null)
        {
            throw new ApplicationException("Roster already has result registered");
        }

        VerifyScores(teamScore, opponentScore);

        ApplyChange(
            new MatchResult4Registered(roster.Id!, roster.Players, teamScore, opponentScore, bitsMatchId));
    }

    private MatchResult4()
    {
        playerPins = new Dictionary<string, List<PinsAndScoreResult>>();
    }

    private string? RosterId { get; set; }

    private int BitsMatchId { get; set; }

    private int OpponentScore { get; set; }

    private int TeamScore { get; set; }

    public async Task<bool> Update(
        Func<TaskBase, Task> publish,
        Roster roster,
        int teamScore,
        int opponentScore,
        int bitsMatchId,
        MatchSerie4[] matchSeries,
        Player[] players)
    {
        // check if anything has changed
        SortedDictionary<string, List<PinsAndScoreResult>> potentiallyNewPlayerPins = new();
        foreach (MatchSerie4 matchSerie in matchSeries)
        {
            foreach (MatchGame4 matchGame in new[] { matchSerie.Game1, matchSerie.Game2, matchSerie.Game3, matchSerie.Game4 })
            {
                if (potentiallyNewPlayerPins.TryGetValue(matchGame.Player, out List<PinsAndScoreResult> list) == false)
                {
                    list = new List<PinsAndScoreResult>();
                    potentiallyNewPlayerPins.Add(matchGame.Player, list);
                }

                list.Add(new PinsAndScoreResult(matchGame.Pins, matchGame.Score, matchSerie.SerieNumber));
            }
        }

        string oldResult = JsonConvert.SerializeObject(
            new SortedDictionary<string, List<PinsAndScoreResult>>(playerPins),
            Formatting.Indented);
        string newResult = JsonConvert.SerializeObject(
            potentiallyNewPlayerPins,
            Formatting.Indented);
        bool pinsOrPlayersDiffer = oldResult != newResult;
        bool scoresDiffer = (teamScore, opponentScore).CompareTo((TeamScore, OpponentScore)) != 0;
        if (pinsOrPlayersDiffer || scoresDiffer)
        {
            MatchResult4Registered @event = new(
                roster.Id!,
                roster.Players,
                teamScore,
                opponentScore,
                bitsMatchId,
                playerPins.Keys.AsEnumerable().ToArray());
            ApplyChange(@event);
            await RegisterSeries(publish, matchSeries, players, null, null);
        }

        return roster.Date.AddDays(5) < SystemTime.UtcNow;
    }

    public void RegisterSerie(MatchSerie4 matchSerie)
    {
        if (matchSerie == null)
        {
            throw new ArgumentNullException(nameof(matchSerie));
        }

        if (rosterPlayers!.Count is not 4 and not 5)
        {
            throw new MatchException("Roster must have 4 or 5 players when registering results");
        }

        VerifyPlayers(matchSerie);

        ApplyChange(new Serie4Registered(matchSerie, BitsMatchId, RosterId!));
        DoAwardMedals(registeredSeries);
    }

    public async Task RegisterSeries(
        Func<TaskBase, Task> publish,
        MatchSerie4[] matchSeries,
        Player[] players,
        string? summaryText,
        string? summaryHtml)
    {
        if (rosterPlayers!.Count is not 4 and not 5)
        {
            throw new MatchException("Roster must have 4 or 5 players when registering results");
        }

        foreach (MatchSerie4 matchSerie in matchSeries)
        {
            VerifyPlayers(matchSerie);

            ApplyChange(new Serie4Registered(matchSerie, BitsMatchId, RosterId!));
            DoAwardMedals(registeredSeries);
        }

        Match4Analyzer analyzer = new(matchSeries, players.ToDictionary(x => x.Id));
        string bodyText = analyzer.GetBodyText();
        if (string.IsNullOrWhiteSpace(summaryText))
        {
            ApplyChange(
                new MatchCommentaryEvent(
                    BitsMatchId,
                    RosterId!,
                    bodyText,
                    bodyText,
                    new string[0]));
        }
        else
        {
            ApplyChange(
                new MatchCommentaryEvent(
                    BitsMatchId,
                    RosterId!,
                    summaryText!,
                    summaryHtml!,
                    new[] { bodyText }));
        }

        await publish.Invoke(
            new MatchRegisteredTask(RosterId!, BitsMatchId, TeamScore, OpponentScore));
    }

    public void AwardMedals()
    {
        if (medalsAwarded)
        {
            throw new ApplicationException("Medals have already been awarded");
        }

        for (int i = 1; i <= registeredSeries; i++)
        {
            DoAwardMedals(i);
        }

        ApplyChange(new MedalsAwarded());
    }

    public void ClearMedals()
    {
        ApplyChange(new ClearMedals(BitsMatchId, RosterId!));
    }

    private static void VerifyScores(int teamScore, int opponentScore)
    {
        if (teamScore is < 0 or > 20)
        {
            throw new ArgumentOutOfRangeException(nameof(teamScore), "Team score must be between 0 and 20");
        }

        if (opponentScore is < 0 or > 20)
        {
            throw new ArgumentOutOfRangeException(nameof(opponentScore), "Opponent score must be between 0 and 20");
        }

        if (teamScore + opponentScore > 20)
        {
            throw new ArgumentException("Team score and opponent score must be at most 20");
        }
    }

    private void DoAwardMedals(int serie)
    {
        foreach (string key in playerPins.Keys)
        {
            PinsAndScoreResult pinsResult = playerPins[key].SingleOrDefault(x => x.SerieNumber == serie);

            if (pinsResult?.Pins >= 270)
            {
                AwardedMedal medal = new(
                    BitsMatchId,
                    RosterId!,
                    key,
                    MedalType.PinsInSerie,
                    pinsResult.Pins);
                ApplyChange(medal);
            }
        }

        if (serie == 4)
        {
            foreach (string key in playerPins.Keys)
            {
                List<PinsAndScoreResult> list = playerPins[key];
                int score = list.Sum(x => x.Score);
                if (score != 4)
                {
                    continue;
                }

                AwardedMedal medal = new(
                    BitsMatchId,
                    RosterId!,
                    key,
                    MedalType.TotalScore,
                    4);
                ApplyChange(medal);
            }
        }
    }

    private void VerifyPlayers(MatchSerie4 matchSerie)
    {
        do
        {
            if (rosterPlayers!.Contains(matchSerie.Game1.Player) == false)
            {
                break;
            }

            if (rosterPlayers.Contains(matchSerie.Game2.Player) == false)
            {
                break;
            }

            if (rosterPlayers.Contains(matchSerie.Game3.Player) == false)
            {
                break;
            }

            if (rosterPlayers.Contains(matchSerie.Game4.Player) == false)
            {
                break;
            }

            return;
        }
        while (false);

        throw new MatchException("Can only register players from roster");
    }

    // events
    private void Apply(MatchResult4Registered e)
    {
        RosterId = e.RosterId;
        TeamScore = e.TeamScore;
        OpponentScore = e.OpponentScore;
        BitsMatchId = e.BitsMatchId;
        rosterPlayers = new HashSet<string>(e.RosterPlayers);
    }

    private void Apply(Serie4Registered e)
    {
        registeredSeries++;
        foreach (MatchGame4 game in new[] { e.MatchSerie.Game1, e.MatchSerie.Game2, e.MatchSerie.Game3, e.MatchSerie.Game4 })
        {
            if (playerPins.ContainsKey(game.Player) == false)
            {
                playerPins.Add(game.Player, new List<PinsAndScoreResult>());
            }

            PinsAndScoreResult pinsAndScoreResult = new(
                game.Pins,
                game.Score,
                registeredSeries);
            playerPins[game.Player].Add(pinsAndScoreResult);
        }
    }

    private void Apply(MedalsAwarded _)
    {
        medalsAwarded = true;
    }

    private void Apply(ClearMedals _)
    {
        medalsAwarded = false;
    }

    private void Apply(AwardedMedal _)
    {
    }

    private void Apply(MatchCommentaryEvent _)
    {
    }
}

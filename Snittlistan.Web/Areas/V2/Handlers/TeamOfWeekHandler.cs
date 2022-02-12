#nullable enable

using EventStoreLite;
using Raven.Client.Documents.Session;
using Snittlistan.Web.Areas.V2.Domain;
using Snittlistan.Web.Areas.V2.Domain.Match.Events;
using Snittlistan.Web.Areas.V2.ReadModels;

namespace Snittlistan.Web.Areas.V2.Handlers;

public class TeamOfWeekHandler :
    IEventHandler<MatchResultRegistered>,
    IEventHandler<SerieRegistered>,
    IEventHandler<MatchResult4Registered>,
    IEventHandler<Serie4Registered>,
    IEventHandler<AwardedMedal>,
    IEventHandler<ClearMedals>
{
    public IDocumentSession DocumentSession { get; set; } = null!;

    public void Handle(MatchResultRegistered e, string aggregateId)
    {
        Roster roster = DocumentSession.Load<Roster>(e.RosterId);
        string id = TeamOfWeek.IdFromBitsMatchId(e.BitsMatchId, e.RosterId);
        TeamOfWeek teamOfWeek = DocumentSession.Load<TeamOfWeek>(id);
        if (teamOfWeek == null)
        {
            teamOfWeek = new TeamOfWeek(e.BitsMatchId, roster.Season, roster.Id!);
            DocumentSession.Store(teamOfWeek);
        }

        teamOfWeek.Reset();
    }

    public void Handle(SerieRegistered e, string aggregateId)
    {
        string id = TeamOfWeek.IdFromBitsMatchId(e.BitsMatchId, e.RosterId);
        TeamOfWeek teamOfWeek = DocumentSession.Load<TeamOfWeek>(id);
        Domain.Match.MatchSerie matchSerie = e.MatchSerie;
        Tuple<string, int, int>[] playerIds = new[]
        {
                Tuple.Create(
                    matchSerie.Table1.Game1.Player,
                    matchSerie.Table1.Score,
                    matchSerie.Table1.Game1.Pins),
                Tuple.Create(
                    matchSerie.Table1.Game2.Player,
                    matchSerie.Table1.Score,
                    matchSerie.Table1.Game2.Pins),
                Tuple.Create(
                    matchSerie.Table2.Game1.Player,
                    matchSerie.Table2.Score,
                    matchSerie.Table2.Game1.Pins),
                Tuple.Create(
                    matchSerie.Table2.Game2.Player,
                    matchSerie.Table2.Score,
                    matchSerie.Table2.Game2.Pins),
                Tuple.Create(
                    matchSerie.Table3.Game1.Player,
                    matchSerie.Table3.Score,
                    matchSerie.Table3.Game1.Pins),
                Tuple.Create(
                    matchSerie.Table3.Game2.Player,
                    matchSerie.Table3.Score,
                    matchSerie.Table3.Game2.Pins),
                Tuple.Create(
                    matchSerie.Table4.Game1.Player,
                    matchSerie.Table4.Score,
                    matchSerie.Table4.Game1.Pins),
                Tuple.Create(
                    matchSerie.Table4.Game2.Player,
                    matchSerie.Table4.Score,
                    matchSerie.Table4.Game2.Pins)
            };
        HashSet<string> uniquePlayerIds = new(playerIds.Select(x => x.Item1));
        Player[] players = DocumentSession.Load<Player>(uniquePlayerIds);
        foreach (Player player in players)
        {
            string playerId = player.Id;
            foreach (Tuple<string, int, int> tuple in playerIds.Where(x => x.Item1 == playerId))
            {
                teamOfWeek.AddResultForPlayer(player, tuple.Item2, tuple.Item3);
            }
        }
    }

    public void Handle(MatchResult4Registered e, string aggregateId)
    {
        Roster roster = DocumentSession.Load<Roster>(e.RosterId);
        string id = TeamOfWeek.IdFromBitsMatchId(e.BitsMatchId, e.RosterId);
        TeamOfWeek teamOfWeek = DocumentSession.Load<TeamOfWeek>(id);
        if (teamOfWeek == null)
        {
            teamOfWeek = new TeamOfWeek(e.BitsMatchId, roster.Season, roster.Id!);
            DocumentSession.Store(teamOfWeek);
        }

        teamOfWeek.Reset();
    }

    public void Handle(Serie4Registered e, string aggregateId)
    {
        string id = TeamOfWeek.IdFromBitsMatchId(e.BitsMatchId, e.RosterId);
        TeamOfWeek teamOfWeek = DocumentSession.Load<TeamOfWeek>(id);
        Domain.Match.MatchSerie4 matchSerie = e.MatchSerie;
        Tuple<string, int, int>[] playerIds = new[]
        {
                Tuple.Create(
                    matchSerie.Game1.Player,
                    matchSerie.Game1.Score,
                    matchSerie.Game1.Pins),
                Tuple.Create(
                    matchSerie.Game2.Player,
                    matchSerie.Game2.Score,
                    matchSerie.Game2.Pins),
                Tuple.Create(
                    matchSerie.Game3.Player,
                    matchSerie.Game3.Score,
                    matchSerie.Game3.Pins),
                Tuple.Create(
                    matchSerie.Game4.Player,
                    matchSerie.Game4.Score,
                    matchSerie.Game4.Pins)
            };
        Player[] players = DocumentSession.Load<Player>(playerIds.Select(x => x.Item1));
        foreach (Player player in players)
        {
            string playerId = player.Id;
            Tuple<string, int, int> tuple = playerIds.Single(x => x.Item1 == playerId);
            teamOfWeek.AddResultForPlayer(player, tuple.Item2, tuple.Item3);
        }
    }

    public void Handle(AwardedMedal e, string aggregateId)
    {
        string id = TeamOfWeek.IdFromBitsMatchId(e.BitsMatchId, e.RosterId);
        TeamOfWeek teamOfWeek = DocumentSession.Load<TeamOfWeek>(id);
        teamOfWeek.AddMedal(new AwardedMedalReadModel(e.Player, e.MedalType, e.Value));
    }

    public void Handle(ClearMedals e, string aggregateId)
    {
        string id = TeamOfWeek.IdFromBitsMatchId(e.BitsMatchId, e.RosterId);
        TeamOfWeek teamOfWeek = DocumentSession.Load<TeamOfWeek>(id);
        teamOfWeek.ClearMedals();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using EventStoreLite;
using Raven.Client;
using Snittlistan.Web.Areas.V2.Domain;
using Snittlistan.Web.Areas.V2.Domain.Match.Events;
using Snittlistan.Web.Areas.V2.ReadModels;

namespace Snittlistan.Web.Areas.V2.Handlers
{
    public class TeamOfWeekHandler :
        IEventHandler<MatchResultRegistered>,
        IEventHandler<SerieRegistered>,
        IEventHandler<MatchResult4Registered>,
        IEventHandler<Serie4Registered>,
        IEventHandler<AwardedMedal>,
        IEventHandler<ClearMedals>
    {
        public IDocumentSession DocumentSession { get; set; }

        public void Handle(MatchResultRegistered e, string aggregateId)
        {
            // find team of week for this roster
            var roster = DocumentSession.Load<Roster>(e.RosterId);

            // TODO: Is this really necessary?
            var teamOfWeek = DocumentSession.Load<TeamOfWeek>(roster.Season);
            if (teamOfWeek != null) return;
            teamOfWeek = new TeamOfWeek(e.BitsMatchId, roster.Season, roster.Turn, roster.Team, roster.TeamLevel);
            DocumentSession.Store(teamOfWeek);
        }

        public void Handle(SerieRegistered e, string aggregateId)
        {
            var id = TeamOfWeek.IdFromBitsMatchId(e.BitsMatchId);
            var teamOfWeek = DocumentSession.Load<TeamOfWeek>(id);
            var matchSerie = e.MatchSerie;
            var playerIds = new[]
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
            var uniquePlayerIds = new HashSet<string>(playerIds.Select(x => x.Item1));
            var players = DocumentSession.Load<Player>(uniquePlayerIds);
            foreach (var player in players)
            {
                var playerId = player.Id;
                foreach (var tuple in playerIds.Where(x => x.Item1 == playerId))
                {
                    teamOfWeek.AddResultForPlayer(player, tuple.Item2, tuple.Item3);
                }
            }
        }

        public void Handle(MatchResult4Registered e, string aggregateId)
        {
            // find team of week for this roster
            var roster = DocumentSession.Load<Roster>(e.RosterId);

            // TODO: Is this really necessary?
            var teamOfWeek = DocumentSession.Load<TeamOfWeek>(roster.Season);
            if (teamOfWeek != null) return;
            teamOfWeek = new TeamOfWeek(e.BitsMatchId, roster.Season, roster.Turn, roster.Team, roster.TeamLevel);
            DocumentSession.Store(teamOfWeek);
        }

        public void Handle(Serie4Registered e, string aggregateId)
        {
            var id = TeamOfWeek.IdFromBitsMatchId(e.BitsMatchId);
            var teamOfWeek = DocumentSession.Load<TeamOfWeek>(id);
            var matchSerie = e.MatchSerie;
            var playerIds = new[]
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
            var players = DocumentSession.Load<Player>(playerIds.Select(x => x.Item1));
            foreach (var player in players)
            {
                var playerId = player.Id;
                var tuple = playerIds.Single(x => x.Item1 == playerId);
                teamOfWeek.AddResultForPlayer(player, tuple.Item2, tuple.Item3);
            }
        }

        public void Handle(AwardedMedal e, string aggregateId)
        {
            var id = TeamOfWeek.IdFromBitsMatchId(e.BitsMatchId);
            var teamOfWeek = DocumentSession.Load<TeamOfWeek>(id);
            teamOfWeek.AddMedal(new AwardedMedalReadModel(e.Player, e.MedalType, e.Value));
        }

        public void Handle(ClearMedals e, string aggregateId)
        {
            var id = TeamOfWeek.IdFromBitsMatchId(e.BitsMatchId);
            var teamOfWeek = DocumentSession.Load<TeamOfWeek>(id);
            teamOfWeek.ClearMedals();
        }
    }
}
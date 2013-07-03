using System;
using System.Linq;
using EventStoreLite;
using Raven.Client;
using Snittlistan.Web.Areas.V2.Domain;
using Snittlistan.Web.Areas.V2.Domain.Match.Events;
using Snittlistan.Web.Areas.V2.ReadModels;

namespace Snittlistan.Web.Areas.V2.Handlers
{
    public class TeamOfWeekHandler : IEventHandler<MatchResultRegistered>,
                                     IEventHandler<SerieRegistered>
    {
        public IDocumentSession DocumentSession { get; set; }

        public void Handle(MatchResultRegistered e, string aggregateId)
        {
            // find team of week for this roster
            var roster = DocumentSession.Load<Roster>(e.RosterId);

            // TODO: Is this really necessary?
            var teamOfWeek = DocumentSession.Load<TeamOfWeek>(roster.Season);
            if (teamOfWeek != null) return;
            teamOfWeek = new TeamOfWeek(e.BitsMatchId, roster.Season, roster.Turn, roster.Team);
            DocumentSession.Store(teamOfWeek);
        }

        public void Handle(SerieRegistered e, string aggregateId)
        {
            var id = TeamOfWeek.IdFromBitsMatchId(e.BitsMatchId);
            var teamOfWeek = DocumentSession.Load<TeamOfWeek>(id);
            var matchSerie = e.MatchSerie;
            var playerIds = new[]
            {
                Tuple.Create(matchSerie.Table1.Game1.Player, matchSerie.Table1.Game1.Pins),
                Tuple.Create(matchSerie.Table1.Game2.Player, matchSerie.Table1.Game2.Pins),
                Tuple.Create(matchSerie.Table2.Game1.Player, matchSerie.Table2.Game1.Pins),
                Tuple.Create(matchSerie.Table2.Game2.Player, matchSerie.Table2.Game2.Pins),
                Tuple.Create(matchSerie.Table3.Game1.Player, matchSerie.Table3.Game1.Pins),
                Tuple.Create(matchSerie.Table3.Game2.Player, matchSerie.Table3.Game2.Pins),
                Tuple.Create(matchSerie.Table4.Game1.Player, matchSerie.Table4.Game1.Pins),
                Tuple.Create(matchSerie.Table4.Game2.Player, matchSerie.Table4.Game2.Pins)
            };
            var players = DocumentSession.Load<Player>(playerIds.Select(x => x.Item1));
            foreach (var player in players)
            {
                var playerId = player.Id;
                var tuple = playerIds.Single(x => x.Item1 == playerId);
                teamOfWeek.AddResultForPlayer(player, tuple.Item2);
            }
        }
    }
}
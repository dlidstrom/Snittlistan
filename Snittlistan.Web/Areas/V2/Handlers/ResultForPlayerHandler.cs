using EventStoreLite;
using Raven.Client;
using Snittlistan.Web.Areas.V2.Domain;
using Snittlistan.Web.Areas.V2.Domain.Match.Events;
using Snittlistan.Web.Areas.V2.ReadModels;

namespace Snittlistan.Web.Areas.V2.Handlers
{
    public class ResultForPlayerHandler :
        IEventHandler<MatchResultRegistered>,
        IEventHandler<MatchResult4Registered>,
        IEventHandler<Serie4Registered>,
        IEventHandler<SerieRegistered>
    {
        public IDocumentSession DocumentSession { get; set; }

        public void Handle(Serie4Registered e, string aggregateId)
        {
            foreach (var game in new[] { e.MatchSerie.Game1, e.MatchSerie.Game2, e.MatchSerie.Game3, e.MatchSerie.Game4 })
            {
                var id = ResultForPlayerReadModel.GetId(game.Player, e.BitsMatchId);
                DocumentSession.Load<ResultForPlayerReadModel>(id).AddGame(game);
            }
        }

        public void Handle(SerieRegistered e, string aggregateId)
        {
            foreach (var table in new[] { e.MatchSerie.Table1, e.MatchSerie.Table2, e.MatchSerie.Table3, e.MatchSerie.Table4 })
            {
                var id1 = ResultForPlayerReadModel.GetId(table.Game1.Player, e.BitsMatchId);
                DocumentSession.Load<ResultForPlayerReadModel>(id1).AddGame(table.Game1);

                var id2 = ResultForPlayerReadModel.GetId(table.Game2.Player, e.BitsMatchId);
                DocumentSession.Load<ResultForPlayerReadModel>(id2).AddGame(table.Game2);
            }
        }

        public void Handle(MatchResultRegistered e, string aggregateId)
        {
            var roster = DocumentSession.Load<Roster>(e.RosterId);
            foreach (var player in e.RosterPlayers)
            {
                var model = new ResultForPlayerReadModel(roster.Season, player, e.BitsMatchId, roster.Date);
                DocumentSession.Store(model);
            }
        }

        public void Handle(MatchResult4Registered e, string aggregateId)
        {
            var roster = DocumentSession.Load<Roster>(e.RosterId);
            foreach (var player in e.RosterPlayers)
            {
                var model = new ResultForPlayerReadModel(roster.Season, player, e.BitsMatchId, roster.Date);
                DocumentSession.Store(model);
            }
        }
    }
}
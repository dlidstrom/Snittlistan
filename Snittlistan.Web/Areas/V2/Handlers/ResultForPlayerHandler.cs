using System.Linq;
using EventStoreLite;
using Raven.Client;
using Snittlistan.Web.Areas.V2.Domain;
using Snittlistan.Web.Areas.V2.Domain.Match;
using Snittlistan.Web.Areas.V2.Domain.Match.Events;
using Snittlistan.Web.Areas.V2.ReadModels;

namespace Snittlistan.Web.Areas.V2.Handlers
{
    public class ResultForPlayerHandler :
        IEventHandler<MatchResultRegistered>,
        IEventHandler<MatchResult4Registered>,
        IEventHandler<Serie4Registered>,
        IEventHandler<SerieRegistered>,
        IEventHandler<ScoreAwarded>
    {
        public IDocumentSession DocumentSession { get; set; }

        public void Handle(MatchResultRegistered e, string aggregateId)
        {
            // need to delete some entries
            var modelsToDelete = DocumentSession.Load<ResultForPlayerReadModel>(e.PreviousPlayerIds.Select(x => ResultForPlayerReadModel.GetId(x, e.BitsMatchId)));
            foreach (var modelToDelete in modelsToDelete)
            {
                DocumentSession.Delete(modelToDelete);
            }

            var roster = DocumentSession.Load<Roster>(e.RosterId);
            foreach (var playerId in e.RosterPlayers)
            {
                var id = ResultForPlayerReadModel.GetId(playerId, e.BitsMatchId);
                var model = DocumentSession.Load<ResultForPlayerReadModel>(id);
                if (model == null)
                {
                    model = new ResultForPlayerReadModel(roster.Season, playerId, e.BitsMatchId, roster.Date);
                    DocumentSession.Store(model);
                }

                model.Clear();
            }
        }

        public void Handle(SerieRegistered e, string aggregateId)
        {
            foreach (var table in new[] { e.MatchSerie.Table1, e.MatchSerie.Table2, e.MatchSerie.Table3, e.MatchSerie.Table4 })
            {
                var id1 = ResultForPlayerReadModel.GetId(table.Game1.Player, e.BitsMatchId);
                DocumentSession.Load<ResultForPlayerReadModel>(id1).AddGame(table.Score, table.Game1);

                var id2 = ResultForPlayerReadModel.GetId(table.Game2.Player, e.BitsMatchId);
                DocumentSession.Load<ResultForPlayerReadModel>(id2).AddGame(table.Score, table.Game2);
            }
        }

        public void Handle(MatchResult4Registered e, string aggregateId)
        {
            // need to delete some entries
            var modelsToDelete = DocumentSession.Load<ResultForPlayerReadModel>(e.PreviousPlayerIds.Select(x => ResultForPlayerReadModel.GetId(x, e.BitsMatchId)));
            foreach (var modelToDelete in modelsToDelete)
            {
                DocumentSession.Delete(modelToDelete);
            }

            var roster = DocumentSession.Load<Roster>(e.RosterId);
            foreach (var playerId in e.RosterPlayers)
            {
                var id = ResultForPlayerReadModel.GetId(playerId, e.BitsMatchId);
                var model = DocumentSession.Load<ResultForPlayerReadModel>(id);
                if (model == null)
                {
                    model = new ResultForPlayerReadModel(roster.Season, playerId, e.BitsMatchId, roster.Date);
                    DocumentSession.Store(model);
                }

                model.Clear();
            }
        }

        public void Handle(Serie4Registered e, string aggregateId)
        {
            foreach (var game in new[] { e.MatchSerie.Game1, e.MatchSerie.Game2, e.MatchSerie.Game3, e.MatchSerie.Game4 })
            {
                var id = ResultForPlayerReadModel.GetId(game.Player, e.BitsMatchId);
                DocumentSession.Load<ResultForPlayerReadModel>(id).AddGame(game);
            }
        }

        public void Handle(ScoreAwarded e, string aggregateId)
        {
            foreach (var playerId in e.PlayerIdToScore.Keys)
            {
                var totalScore = e.PlayerIdToScore[playerId];
                var id = ResultForPlayerReadModel.GetId(playerId, e.BitsMatchId);
                DocumentSession.Load<ResultForPlayerReadModel>(id).SetTotalScore(totalScore);
            }
        }
    }
}
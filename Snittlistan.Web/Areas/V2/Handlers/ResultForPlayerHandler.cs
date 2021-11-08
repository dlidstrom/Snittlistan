#nullable enable

namespace Snittlistan.Web.Areas.V2.Handlers
{
    using System.Collections.Generic;
    using System.Linq;
    using EventStoreLite;
    using Raven.Client;
    using Snittlistan.Web.Areas.V2.Domain;
    using Snittlistan.Web.Areas.V2.Domain.Match;
    using Snittlistan.Web.Areas.V2.Domain.Match.Events;
    using Snittlistan.Web.Areas.V2.ReadModels;

    public class ResultForPlayerHandler :
        IEventHandler<MatchResultRegistered>,
        IEventHandler<MatchResult4Registered>,
        IEventHandler<Serie4Registered>,
        IEventHandler<SerieRegistered>,
        IEventHandler<ScoreAwarded>
    {
        public IDocumentSession DocumentSession { get; set; } = null!;

        public void Handle(MatchResultRegistered e, string aggregateId)
        {
            // need to delete some entries
            ResultForPlayerReadModel[] modelsToDelete = DocumentSession.Load<ResultForPlayerReadModel>(
                e.PreviousPlayerIds.Select(x => ResultForPlayerReadModel.GetId(x, e.BitsMatchId, e.RosterId)));
            HashSet<string> toKeep = new(e.RosterPlayers.Select(x => ResultForPlayerReadModel.GetId(x, e.BitsMatchId, e.RosterId)));
            foreach (ResultForPlayerReadModel modelToDelete in modelsToDelete)
            {
                if (toKeep.Contains(modelToDelete.Id) == false)
                {
                    DocumentSession.Delete(modelToDelete);
                }
            }

            Roster roster = DocumentSession.Load<Roster>(e.RosterId);
            foreach (string playerId in e.RosterPlayers)
            {
                string id = ResultForPlayerReadModel.GetId(playerId, e.BitsMatchId, e.RosterId);
                ResultForPlayerReadModel model = DocumentSession.Load<ResultForPlayerReadModel>(id);
                if (model == null)
                {
                    model = new ResultForPlayerReadModel(roster.Season, playerId, e.BitsMatchId, roster.Id, roster.Date);
                    DocumentSession.Store(model);
                }

                model.Clear();
            }
        }

        public void Handle(SerieRegistered e, string aggregateId)
        {
            foreach (MatchTable table in new[] { e.MatchSerie.Table1, e.MatchSerie.Table2, e.MatchSerie.Table3, e.MatchSerie.Table4 })
            {
                string id1 = ResultForPlayerReadModel.GetId(table.Game1.Player, e.BitsMatchId, e.RosterId);
                DocumentSession.Load<ResultForPlayerReadModel>(id1).AddGame(table.Score, table.Game1);

                string id2 = ResultForPlayerReadModel.GetId(table.Game2.Player, e.BitsMatchId, e.RosterId);
                DocumentSession.Load<ResultForPlayerReadModel>(id2).AddGame(table.Score, table.Game2);
            }
        }

        public void Handle(MatchResult4Registered e, string aggregateId)
        {
            // need to delete some entries
            ResultForPlayerReadModel[] modelsToDelete = DocumentSession.Load<ResultForPlayerReadModel>(e.PreviousPlayerIds.Select(x => ResultForPlayerReadModel.GetId(x, e.BitsMatchId, e.RosterId)));
            HashSet<string> toKeep = new(e.RosterPlayers.Select(x => ResultForPlayerReadModel.GetId(x, e.BitsMatchId, e.RosterId)));
            foreach (ResultForPlayerReadModel modelToDelete in modelsToDelete)
            {
                if (toKeep.Contains(modelToDelete.Id) == false)
                {
                    DocumentSession.Delete(modelToDelete);
                }
            }

            Roster roster = DocumentSession.Load<Roster>(e.RosterId);
            foreach (string playerId in e.RosterPlayers)
            {
                string id = ResultForPlayerReadModel.GetId(playerId, e.BitsMatchId, e.RosterId);
                ResultForPlayerReadModel model = DocumentSession.Load<ResultForPlayerReadModel>(id);
                if (model == null)
                {
                    model = new ResultForPlayerReadModel(roster.Season, playerId, e.BitsMatchId, roster.Id, roster.Date);
                    DocumentSession.Store(model);
                }

                model.Clear();
            }
        }

        public void Handle(Serie4Registered e, string aggregateId)
        {
            foreach (MatchGame4 game in new[] { e.MatchSerie.Game1, e.MatchSerie.Game2, e.MatchSerie.Game3, e.MatchSerie.Game4 })
            {
                string id = ResultForPlayerReadModel.GetId(game.Player, e.BitsMatchId, e.RosterId);
                DocumentSession.Load<ResultForPlayerReadModel>(id).AddGame(game);
            }
        }

        public void Handle(ScoreAwarded e, string aggregateId)
        {
            foreach (string playerId in e.PlayerIdToScore.Keys)
            {
                int totalScore = e.PlayerIdToScore[playerId];
                string id = ResultForPlayerReadModel.GetId(playerId, e.BitsMatchId, e.RosterId);
                DocumentSession.Load<ResultForPlayerReadModel>(id).SetTotalScore(totalScore);
            }
        }
    }
}

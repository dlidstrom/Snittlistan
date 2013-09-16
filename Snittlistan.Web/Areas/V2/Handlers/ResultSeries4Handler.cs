using System.Collections.Generic;
using System.Linq;
using EventStoreLite;
using Raven.Client;
using Snittlistan.Web.Areas.V2.Domain;
using Snittlistan.Web.Areas.V2.Domain.Match;
using Snittlistan.Web.Areas.V2.Domain.Match.Events;
using Snittlistan.Web.Areas.V2.ReadModels;

namespace Snittlistan.Web.Areas.V2.Handlers
{
    public class ResultSeries4Handler : IEventHandler<Serie4Registered>
    {
        public IDocumentSession DocumentSession { get; set; }

        public void Handle(Serie4Registered e, string aggregateId)
        {
            var id = ResultSeries4ReadModel.IdFromBitsMatchId(e.BitsMatchId);
            var results = DocumentSession.Load<ResultSeries4ReadModel>(id);
            if (results == null)
            {
                results = new ResultSeries4ReadModel { Id = id };
                DocumentSession.Store(results);
            }

            var matchSerie = e.MatchSerie;
            var playerIds = new[]
            {
                matchSerie.Game1.Player,
                matchSerie.Game2.Player,
                matchSerie.Game3.Player,
                matchSerie.Game4.Player
            };

            var players = DocumentSession.Load<Player>(playerIds).ToDictionary(x => x.Id);
            var game1 = CreateGame(players, matchSerie.Game1);
            var game2 = CreateGame(players, matchSerie.Game2);
            var game3 = CreateGame(players, matchSerie.Game3);
            var game4 = CreateGame(players, matchSerie.Game4);

            results.Series.Add(new ResultSeries4ReadModel.Serie
            {
                Games = new List<ResultSeries4ReadModel.Game>
                {
                    game1,
                    game2,
                    game3,
                    game4
                }
            });
        }

        private static ResultSeries4ReadModel.Game CreateGame(
            IReadOnlyDictionary<string, Player> players, MatchGame4 matchGame)
        {
            var game = new ResultSeries4ReadModel.Game
            {
                Score = matchGame.Score,
                Player = players[matchGame.Player].Name,
                Pins = matchGame.Pins
            };
            return game;
        }
    }
}
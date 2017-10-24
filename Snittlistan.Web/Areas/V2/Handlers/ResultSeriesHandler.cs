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
    public class ResultSeriesHandler :
        IEventHandler<SerieRegistered>
    {
        public IDocumentSession DocumentSession { get; set; }

        public void Handle(SerieRegistered e, string aggregateId)
        {
            var id = ResultSeriesReadModel.IdFromBitsMatchId(e.BitsMatchId);
            var results = DocumentSession.Load<ResultSeriesReadModel>(id);
            if (results == null)
            {
                results = new ResultSeriesReadModel { Id = id };
                DocumentSession.Store(results);
            }

            var matchSerie = e.MatchSerie;
            var playerIds = new HashSet<string>
            {
                matchSerie.Table1.Game1.Player,
                matchSerie.Table1.Game2.Player,
                matchSerie.Table2.Game1.Player,
                matchSerie.Table2.Game2.Player,
                matchSerie.Table3.Game1.Player,
                matchSerie.Table3.Game2.Player,
                matchSerie.Table4.Game1.Player,
                matchSerie.Table4.Game2.Player
            };

            var players = DocumentSession.Load<Player>(playerIds).ToDictionary(x => x.Id);
            var table1 = CreateTable(players, matchSerie.Table1);
            var table2 = CreateTable(players, matchSerie.Table2);
            var table3 = CreateTable(players, matchSerie.Table3);
            var table4 = CreateTable(players, matchSerie.Table4);

            results.Series.Add(new ResultSeriesReadModel.Serie
                               {
                                   Tables = new List<ResultSeriesReadModel.Table>
                                            {
                                                table1,
                                                table2,
                                                table3,
                                                table4
                                            }
                               });
        }

        private static ResultSeriesReadModel.Table CreateTable(
            IReadOnlyDictionary<string, Player> players,
            MatchTable matchTable)
        {
            var table = new ResultSeriesReadModel.Table
                         {
                             Score = matchTable.Score,
                             Game1 = new ResultSeriesReadModel.Game
                                     {
                                         Player =
                                             players[matchTable.Game1.Player].Name,
                                         Pins =
                                             matchTable
                                             .Game1.Pins,
                                         Strikes =
                                             matchTable
                                             .Game1.Strikes,
                                         Spares =
                                             matchTable
                                             .Game1.Spares
                                     },
                             Game2 = new ResultSeriesReadModel.Game
                                     {
                                         Player =
                                             players[matchTable.Game2.Player].Name,
                                         Pins =
                                             matchTable.Game2.Pins,
                                         Strikes =
                                             matchTable.Game2.Strikes,
                                         Spares =
                                             matchTable.Game2.Spares
                                     }
                         };
            return table;
        }
    }
}
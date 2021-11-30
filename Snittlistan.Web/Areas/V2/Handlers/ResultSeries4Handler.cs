using EventStoreLite;
using Raven.Client;
using Snittlistan.Web.Areas.V2.Domain;
using Snittlistan.Web.Areas.V2.Domain.Match;
using Snittlistan.Web.Areas.V2.Domain.Match.Events;
using Snittlistan.Web.Areas.V2.ReadModels;

#nullable enable

namespace Snittlistan.Web.Areas.V2.Handlers;
public class ResultSeries4Handler :
    IEventHandler<MatchResult4Registered>,
    IEventHandler<Serie4Registered>
{
    public IDocumentSession DocumentSession { get; set; } = null!;

    public void Handle(MatchResult4Registered e, string aggregateId)
    {
        string id = ResultSeries4ReadModel.IdFromBitsMatchId(e.BitsMatchId, e.RosterId);
        ResultSeries4ReadModel results = DocumentSession.Load<ResultSeries4ReadModel>(id);
        if (results == null)
        {
            results = new ResultSeries4ReadModel { Id = id };
            DocumentSession.Store(results);
        }

        results.Clear();
    }

    public void Handle(Serie4Registered e, string aggregateId)
    {
        string id = ResultSeries4ReadModel.IdFromBitsMatchId(e.BitsMatchId, e.RosterId);
        ResultSeries4ReadModel results = DocumentSession.Load<ResultSeries4ReadModel>(id);

        MatchSerie4 matchSerie = e.MatchSerie;
        string[] playerIds = new[]
        {
                matchSerie.Game1.Player,
                matchSerie.Game2.Player,
                matchSerie.Game3.Player,
                matchSerie.Game4.Player
            };

        Dictionary<string, Player> players = DocumentSession.Load<Player>(playerIds).ToDictionary(x => x.Id);
        ResultSeries4ReadModel.Game game1 = CreateGame(players, matchSerie.Game1);
        ResultSeries4ReadModel.Game game2 = CreateGame(players, matchSerie.Game2);
        ResultSeries4ReadModel.Game game3 = CreateGame(players, matchSerie.Game3);
        ResultSeries4ReadModel.Game game4 = CreateGame(players, matchSerie.Game4);

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
        IReadOnlyDictionary<string, Player> players,
        MatchGame4 matchGame)
    {
        ResultSeries4ReadModel.Game game = new()
        {
            Score = matchGame.Score,
            Player = players[matchGame.Player].Name,
            Pins = matchGame.Pins
        };
        return game;
    }
}

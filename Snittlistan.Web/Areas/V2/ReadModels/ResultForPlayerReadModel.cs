using System;
using EventStoreLite;
using Snittlistan.Web.Areas.V2.Domain.Match;

namespace Snittlistan.Web.Areas.V2.ReadModels
{
    public class ResultForPlayerReadModel : IReadModel
    {
        public ResultForPlayerReadModel(int season, string playerId, int bitsMatchId, DateTime date)
        {
            Season = season;
            PlayerId = playerId;
            BitsMatchId = bitsMatchId;
            Id = GetId(playerId, bitsMatchId);
            Date = date;
        }

        public string Id { get; private set; }

        public int Season { get; private set; }

        public string PlayerId { get; private set; }

        public int BitsMatchId { get; private set; }

        public DateTime Date { get; set; }

        public int TotalPins { get; private set; }

        public int TotalSeries { get; private set; }

        public static string GetId(string playerId, int bitsMatchId)
        {
            return string.Format("ResultForPlayer-{0}-{1}", playerId, bitsMatchId);
        }

        public void AddGame(MatchGame4 game)
        {
            TotalSeries++;
            TotalPins += game.Pins;
        }

        public void AddGame(MatchGame game)
        {
            TotalSeries++;
            TotalPins += game.Pins;
        }
    }
}
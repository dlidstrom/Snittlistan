namespace Snittlistan.Web.Areas.V2.ReadModels
{
    using System;
    using EventStoreLite;
    using Snittlistan.Web.Areas.V2.Domain.Match;

    public class ResultForPlayerReadModel : IReadModel
    {
        public ResultForPlayerReadModel(
            int season,
            string playerId,
            int bitsMatchId,
            string rosterId,
            DateTime date)
        {
            Season = season;
            PlayerId = playerId;
            BitsMatchId = bitsMatchId;
            Id = GetId(playerId, bitsMatchId, rosterId);
            Date = date;
        }

        public string Id { get; private set; }

        public int Season { get; private set; }

        public string PlayerId { get; private set; }

        public int BitsMatchId { get; private set; }

        public DateTime Date { get; private set; }

        public int TotalPins { get; private set; }

        public int TotalScore { get; private set; }

        public int TotalSeries { get; private set; }

        public static string GetId(string playerId, int bitsMatchId, string rosterId)
        {
            if (bitsMatchId != 0)
                return $"ResultForPlayer-{playerId}-{bitsMatchId}";
            return $"ResultForPlayer-{playerId}-R{rosterId.Substring(8)}";
        }

        public void AddGame(MatchGame4 game)
        {
            TotalSeries++;
            TotalPins += game.Pins;
            TotalScore += game.Score;
        }

        public void AddGame(int score, MatchGame game)
        {
            TotalSeries++;
            TotalPins += game.Pins;
            TotalScore += score;
        }

        public void SetTotalScore(int totalScore)
        {
            TotalScore = totalScore;
        }

        public void Clear()
        {
            TotalSeries = 0;
            TotalPins = 0;
            TotalScore = 0;
        }
    }
}
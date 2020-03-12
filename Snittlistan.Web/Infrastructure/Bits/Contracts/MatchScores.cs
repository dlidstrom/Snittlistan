namespace Snittlistan.Web.Infrastructure.Bits.Contracts
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public class MatchScores
    {
        [JsonProperty("series")]
        public Series[] Series { get; set; }

        [JsonProperty("serieNames")]
        public long[] SerieNames { get; set; }

        [JsonProperty("boardNames")]
        public string[] BoardNames { get; set; }

        [JsonProperty("scoreKvpList")]
        public Dictionary<string, Score> ScoreKvpList { get; set; }

        [JsonProperty("boardColSpan")]
        public long BoardColSpan { get; set; }

        public static MatchScores FromJson(string json) => JsonConvert.DeserializeObject<MatchScores>(json);
    }

    public class Score
    {
        [JsonProperty("playerName")]
        public string PlayerName { get; set; }

        [JsonProperty("score")]
        public long ScoreScore { get; set; }

        [JsonProperty("laneScore")]
        public long LaneScore { get; set; }

        [JsonProperty("scoreId")]
        public string ScoreId { get; set; }
    }

    public class Series
    {
        [JsonProperty("boards")]
        public Board[] Boards { get; set; }

        [JsonProperty("serieId")]
        public object SerieId { get; set; }

        [JsonProperty("serieName")]
        public object SerieName { get; set; }
    }

    public class Board
    {
        [JsonProperty("scores")]
        public Score[] Scores { get; set; }

        [JsonProperty("boardId")]
        public object BoardId { get; set; }

        [JsonProperty("boardName")]
        public object BoardName { get; set; }
    }
}

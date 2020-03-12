namespace Snittlistan.Web.Infrastructure.Bits.Contracts
{
    using Newtonsoft.Json;

    public partial class HeadResultInfo
    {
        [JsonProperty("matchHeadHomeTeamResult")]
        public int MatchHeadHomeTeamResult { get; set; }

        [JsonProperty("matchHeadAwayTeamResult")]
        public int MatchHeadAwayTeamResult { get; set; }

        [JsonProperty("matchHeadHomeTotalScore")]
        public int MatchHeadHomeTotalScore { get; set; }

        [JsonProperty("matchHeadAwayTotalScore")]
        public int MatchHeadAwayTotalScore { get; set; }

        [JsonProperty("matchHeadHomeTotalRp")]
        public int MatchHeadHomeTotalRp { get; set; }

        [JsonProperty("matchHeadAwayTotalRp")]
        public int MatchHeadAwayTotalRp { get; set; }

        [JsonProperty("homeHeadDetails")]
        public HeadDetail[] HomeHeadDetails { get; set; }

        [JsonProperty("awayHeadDetails")]
        public HeadDetail[] AwayHeadDetails { get; set; }

        [JsonProperty("matchHeadHomeTeamScoreRound1")]
        public int MatchHeadHomeTeamScoreRound1 { get; set; }

        [JsonProperty("matchHeadHomeTeamScoreRound2")]
        public int MatchHeadHomeTeamScoreRound2 { get; set; }

        [JsonProperty("matchHeadHomeTeamScoreRound3")]
        public int MatchHeadHomeTeamScoreRound3 { get; set; }

        [JsonProperty("matchHeadHomeTeamScoreRound4")]
        public int MatchHeadHomeTeamScoreRound4 { get; set; }

        [JsonProperty("matchHeadHomeTeamRankPointRound1")]
        public int MatchHeadHomeTeamRankPointRound1 { get; set; }

        [JsonProperty("matchHeadHomeTeamRankPointRound2")]
        public int MatchHeadHomeTeamRankPointRound2 { get; set; }

        [JsonProperty("matchHeadHomeTeamRankPointRound3")]
        public int MatchHeadHomeTeamRankPointRound3 { get; set; }

        [JsonProperty("matchHeadHomeTeamRankPointRound4")]
        public int MatchHeadHomeTeamRankPointRound4 { get; set; }

        [JsonProperty("matchHeadAwayTeamScoreRound1")]
        public int MatchHeadAwayTeamScoreRound1 { get; set; }

        [JsonProperty("matchHeadAwayTeamScoreRound2")]
        public int MatchHeadAwayTeamScoreRound2 { get; set; }

        [JsonProperty("matchHeadAwayTeamScoreRound3")]
        public int MatchHeadAwayTeamScoreRound3 { get; set; }

        [JsonProperty("matchHeadAwayTeamScoreRound4")]
        public int MatchHeadAwayTeamScoreRound4 { get; set; }

        [JsonProperty("matchHeadAwayTeamRankPointRound1")]
        public int MatchHeadAwayTeamRankPointRound1 { get; set; }

        [JsonProperty("matchHeadAwayTeamRankPointRound2")]
        public int MatchHeadAwayTeamRankPointRound2 { get; set; }

        [JsonProperty("matchHeadAwayTeamRankPointRound3")]
        public int MatchHeadAwayTeamRankPointRound3 { get; set; }

        [JsonProperty("matchHeadAwayTeamRankPointRound4")]
        public int MatchHeadAwayTeamRankPointRound4 { get; set; }

        [JsonProperty("matchHeadHomeTeamScore")]
        public int MatchHeadHomeTeamScore { get; set; }

        [JsonProperty("matchHeadAwayTeamScore")]
        public int MatchHeadAwayTeamScore { get; set; }

        [JsonProperty("matchHeadHomeTeamRankPoints")]
        public int MatchHeadHomeTeamRankPoints { get; set; }

        [JsonProperty("matchHeadAwayTeamRankPoints")]
        public int MatchHeadAwayTeamRankPoints { get; set; }
    }

    public class HeadDetail
    {
        [JsonProperty("squadId")]
        public int SquadId { get; set; }

        [JsonProperty("teamScore")]
        public int TeamScore { get; set; }

        [JsonProperty("teamRP")]
        public int TeamRp { get; set; }
    }

    public partial class HeadResultInfo
    {
        public static HeadResultInfo FromJson(string json) => JsonConvert.DeserializeObject<HeadResultInfo>(json, Converter.Settings);
    }
}

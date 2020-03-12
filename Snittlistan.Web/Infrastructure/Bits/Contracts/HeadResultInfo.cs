namespace Snittlistan.Web.Infrastructure.Bits.Contracts
{
    using Newtonsoft.Json;

    public partial class HeadResultInfo
    {
        [JsonProperty("matchHeadHomeTeamResult")]
        public long MatchHeadHomeTeamResult { get; set; }

        [JsonProperty("matchHeadAwayTeamResult")]
        public long MatchHeadAwayTeamResult { get; set; }

        [JsonProperty("matchHeadHomeTotalScore")]
        public long MatchHeadHomeTotalScore { get; set; }

        [JsonProperty("matchHeadAwayTotalScore")]
        public long MatchHeadAwayTotalScore { get; set; }

        [JsonProperty("matchHeadHomeTotalRp")]
        public long MatchHeadHomeTotalRp { get; set; }

        [JsonProperty("matchHeadAwayTotalRp")]
        public long MatchHeadAwayTotalRp { get; set; }

        [JsonProperty("homeHeadDetails")]
        public HeadDetail[] HomeHeadDetails { get; set; }

        [JsonProperty("awayHeadDetails")]
        public HeadDetail[] AwayHeadDetails { get; set; }

        [JsonProperty("matchHeadHomeTeamScoreRound1")]
        public long MatchHeadHomeTeamScoreRound1 { get; set; }

        [JsonProperty("matchHeadHomeTeamScoreRound2")]
        public long MatchHeadHomeTeamScoreRound2 { get; set; }

        [JsonProperty("matchHeadHomeTeamScoreRound3")]
        public long MatchHeadHomeTeamScoreRound3 { get; set; }

        [JsonProperty("matchHeadHomeTeamScoreRound4")]
        public long MatchHeadHomeTeamScoreRound4 { get; set; }

        [JsonProperty("matchHeadHomeTeamRankPointRound1")]
        public long MatchHeadHomeTeamRankPointRound1 { get; set; }

        [JsonProperty("matchHeadHomeTeamRankPointRound2")]
        public long MatchHeadHomeTeamRankPointRound2 { get; set; }

        [JsonProperty("matchHeadHomeTeamRankPointRound3")]
        public long MatchHeadHomeTeamRankPointRound3 { get; set; }

        [JsonProperty("matchHeadHomeTeamRankPointRound4")]
        public long MatchHeadHomeTeamRankPointRound4 { get; set; }

        [JsonProperty("matchHeadAwayTeamScoreRound1")]
        public long MatchHeadAwayTeamScoreRound1 { get; set; }

        [JsonProperty("matchHeadAwayTeamScoreRound2")]
        public long MatchHeadAwayTeamScoreRound2 { get; set; }

        [JsonProperty("matchHeadAwayTeamScoreRound3")]
        public long MatchHeadAwayTeamScoreRound3 { get; set; }

        [JsonProperty("matchHeadAwayTeamScoreRound4")]
        public long MatchHeadAwayTeamScoreRound4 { get; set; }

        [JsonProperty("matchHeadAwayTeamRankPointRound1")]
        public long MatchHeadAwayTeamRankPointRound1 { get; set; }

        [JsonProperty("matchHeadAwayTeamRankPointRound2")]
        public long MatchHeadAwayTeamRankPointRound2 { get; set; }

        [JsonProperty("matchHeadAwayTeamRankPointRound3")]
        public long MatchHeadAwayTeamRankPointRound3 { get; set; }

        [JsonProperty("matchHeadAwayTeamRankPointRound4")]
        public long MatchHeadAwayTeamRankPointRound4 { get; set; }

        [JsonProperty("matchHeadHomeTeamScore")]
        public long MatchHeadHomeTeamScore { get; set; }

        [JsonProperty("matchHeadAwayTeamScore")]
        public long MatchHeadAwayTeamScore { get; set; }

        [JsonProperty("matchHeadHomeTeamRankPoints")]
        public long MatchHeadHomeTeamRankPoints { get; set; }

        [JsonProperty("matchHeadAwayTeamRankPoints")]
        public long MatchHeadAwayTeamRankPoints { get; set; }
    }

    public class HeadDetail
    {
        [JsonProperty("squadId")]
        public long SquadId { get; set; }

        [JsonProperty("teamScore")]
        public long TeamScore { get; set; }

        [JsonProperty("teamRP")]
        public long TeamRp { get; set; }
    }

    public partial class HeadResultInfo
    {
        public static HeadResultInfo FromJson(string json) => JsonConvert.DeserializeObject<HeadResultInfo>(json, Converter.Settings);
    }
}

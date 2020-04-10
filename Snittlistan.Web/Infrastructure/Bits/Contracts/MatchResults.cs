namespace Snittlistan.Web.Infrastructure.Bits.Contracts
{
    using System;
    using Newtonsoft.Json;

    public class MatchResults
    {
        [JsonProperty("playerListHome")]
        public PlayerList[] PlayerListHome { get; set; }

        [JsonProperty("playerListAway")]
        public PlayerList[] PlayerListAway { get; set; }
    }

    public class PlayerList
    {
        [JsonProperty("player")]
        public string Player { get; set; }

        [JsonProperty("licNbr")]
        public string LicNbr { get; set; }

        [JsonProperty("homeOrAwayTeam")]
        public HomeOrAwayTeam HomeOrAwayTeam { get; set; }

        [JsonProperty("result1")]
        public long Result1 { get; set; }

        [JsonProperty("result2")]
        public long Result2 { get; set; }

        [JsonProperty("result3")]
        public long Result3 { get; set; }

        [JsonProperty("result4")]
        public long Result4 { get; set; }

        [JsonProperty("hcp")]
        public long Hcp { get; set; }

        [JsonProperty("totalResultWithoutHcp")]
        public long TotalResultWithoutHcp { get; set; }

        [JsonProperty("totalSeries")]
        public long TotalSeries { get; set; }

        [JsonProperty("lanePoint")]
        public long LanePoint { get; set; }

        [JsonProperty("laneRankPoints")]
        public double LaneRankPoints { get; set; }

        [JsonProperty("place")]
        public long Place { get; set; }

        [JsonProperty("totalResult")]
        public long TotalResult { get; set; }

        [JsonProperty("rankPoints")]
        public double RankPoints { get; set; }

        [JsonProperty("totalPoints")]
        public double TotalPoints { get; set; }
    }

    public enum HomeOrAwayTeam { A, H };

    internal class HomeOrAwayTeamConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(HomeOrAwayTeam) || t == typeof(HomeOrAwayTeam?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "A":
                    return HomeOrAwayTeam.A;
                case "H":
                    return HomeOrAwayTeam.H;
            }
            throw new Exception("Cannot unmarshal type HomeOrAwayTeam");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (HomeOrAwayTeam)untypedValue;
            switch (value)
            {
                case HomeOrAwayTeam.A:
                    serializer.Serialize(writer, "A");
                    return;
                case HomeOrAwayTeam.H:
                    serializer.Serialize(writer, "H");
                    return;
            }
            throw new Exception("Cannot marshal type HomeOrAwayTeam");
        }

        public static readonly HomeOrAwayTeamConverter Singleton = new HomeOrAwayTeamConverter();
    }
}

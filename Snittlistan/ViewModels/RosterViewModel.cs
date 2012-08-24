namespace Snittlistan.ViewModels
{
    using Raven.Imports.Newtonsoft.Json;

    public class RosterViewModel
    {
        [JsonProperty(PropertyName = "team")]
        public string Team { get; set; }

        [JsonProperty(PropertyName = "team_level")]
        public string TeamLevel { get; set; }

        [JsonProperty(PropertyName = "location")]
        public string Location { get; set; }

        [JsonProperty(PropertyName = "opponent")]
        public string Opponent { get; set; }

        [JsonProperty(PropertyName = "date")]
        public string Date { get; set; }

        [JsonProperty(PropertyName = "declinedClass")]
        public string DeclinedClass { get; set; }

        [JsonProperty(PropertyName = "declinedCount")]
        public int DeclinedCount { get; set; }

        [JsonProperty(PropertyName = "declinedNames")]
        public string DeclinedNames { get; set; }
    }
}
namespace Snittlistan.Web.ViewModels
{
    using Raven.Imports.Newtonsoft.Json;

    public class TurnsViewModel
    {
        [JsonProperty(PropertyName = "season")]
        public string Season { get; set; }

        [JsonProperty(PropertyName = "turn")]
        public int Turn { get; set; }

        [JsonProperty(PropertyName = "startDate")]
        public string StartDate { get; set; }

        [JsonProperty(PropertyName = "endDate")]
        public string EndDate { get; set; }

        [JsonProperty(PropertyName = "rosters")]
        public RosterViewModel[] Rosters { get; set; }
    }
}
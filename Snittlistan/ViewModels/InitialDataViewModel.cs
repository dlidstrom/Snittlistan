namespace Snittlistan.ViewModels
{
    using Raven.Imports.Newtonsoft.Json;

    public class InitialDataViewModel
    {
        [JsonProperty(PropertyName = "turns")]
        public TurnsViewModel[] Turns { get; set; }

        [JsonProperty(PropertyName = "players")]
        public PlayerViewModel[] Players { get; set; }
    }
}
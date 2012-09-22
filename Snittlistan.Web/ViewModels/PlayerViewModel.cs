namespace Snittlistan.Web.ViewModels
{
    using Raven.Imports.Newtonsoft.Json;

    public class PlayerViewModel
    {
        [JsonProperty(PropertyName = "index")]
        public int Index { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
    }
}
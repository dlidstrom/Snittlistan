namespace Snittlistan.ViewModels
{
    using Raven.Imports.Newtonsoft.Json;

    public class PlayerViewModel
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
    }
}
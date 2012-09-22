namespace Snittlistan.Web.ViewModels
{
    using Raven.Imports.Newtonsoft.Json;

    public class SessionViewModel
    {
        [JsonProperty(PropertyName = "isAuthenticated")]
        public bool IsAuthenticated { get; set; }

        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }
    }
}
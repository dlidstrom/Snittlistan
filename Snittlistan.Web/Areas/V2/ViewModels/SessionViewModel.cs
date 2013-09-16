using Raven.Imports.Newtonsoft.Json;

namespace Snittlistan.Web.Areas.V2.ViewModels
{
    public class SessionViewModel
    {
        [JsonProperty(PropertyName = "isAuthenticated")]
        public bool IsAuthenticated { get; set; }

        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }
    }
}
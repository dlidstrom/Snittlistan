using System;
using Raven.Imports.Newtonsoft.Json;

namespace Snittlistan.Web.Models
{
    public class WebsiteConfig
    {
        public const string GlobalId = "WebsiteConfig";

        [JsonConstructor]
        private WebsiteConfig()
        {
            TeamNames = new string[0];
        }

        public WebsiteConfig(string[] teamNames, bool hasV1)
        {
            if (teamNames == null) throw new ArgumentNullException(nameof(teamNames));
            Id = GlobalId;
            TeamNames = teamNames;
            HasV1 = hasV1;
        }

        public string Id { get; }

        public string[] TeamNames { get; }

        public bool HasV1 { get; }
    }
}
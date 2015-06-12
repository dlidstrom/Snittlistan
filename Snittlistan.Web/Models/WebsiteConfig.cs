using System;

namespace Snittlistan.Web.Models
{
    public class WebsiteConfig
    {
        public const string GlobalId = "WebsiteConfig";

        public WebsiteConfig()
        {
            Id = GlobalId;
            IndexCreatedVersion = string.Empty;
        }

        public string Id { get; private set; }

        public string IndexCreatedVersion { get; private set; }

        public void SetIndexCreatedVersion(string version)
        {
            if (version == null) throw new ArgumentNullException("version");
            IndexCreatedVersion = version;
        }
    }
}
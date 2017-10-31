namespace Snittlistan.Web.Models
{
    public class WebsiteConfig
    {
        public const string GlobalId = "WebsiteConfig";

        public WebsiteConfig(string[] teamNames, bool hasV1)
        {
            Id = GlobalId;
            TeamNames = teamNames ?? new string[0];
            HasV1 = hasV1;
        }

        public string Id { get; }

        public string[] TeamNames { get; }

        public bool HasV1 { get; }
    }
}
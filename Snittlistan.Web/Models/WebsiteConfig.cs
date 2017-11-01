using System.Collections.Generic;
using System.Linq;
using Raven.Imports.Newtonsoft.Json;

namespace Snittlistan.Web.Models
{
    public class WebsiteConfig
    {
        public const string GlobalId = "WebsiteConfig";

        public WebsiteConfig(TeamNameAndLevel[] teamNamesAndLevels, bool hasV1)
        {
            Id = GlobalId;
            TeamNamesAndLevels = teamNamesAndLevels ?? new TeamNameAndLevel[0];
            HasV1 = hasV1;
        }

        public string Id { get; }

        public TeamNameAndLevel[] TeamNamesAndLevels { get; }

        public bool HasV1 { get; }

        public HashSet<string> GetTeamNames()
        {
            return new HashSet<string>(TeamNamesAndLevels.Select(x => x.TeamName));
        }

        public class TeamNameAndLevel
        {
            public TeamNameAndLevel(string teamName, string level)
            {
                TeamName = teamName;
                Level = level;
            }

            public string TeamName { get; }

            public string Level { get; }

            [JsonIgnore]
            public string FormattedForOption => $"{TeamName};{Level}";
        }
    }
}
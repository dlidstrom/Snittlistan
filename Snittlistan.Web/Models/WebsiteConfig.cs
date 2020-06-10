namespace Snittlistan.Web.Models
{
    using Raven.Imports.Newtonsoft.Json;

    public class WebsiteConfig
    {
        public const string GlobalId = "WebsiteConfig";

        public WebsiteConfig(TeamNameAndLevel[] teamNamesAndLevels, bool hasV1, int clubId, int seasonId)
        {
            Id = GlobalId;
            TeamNamesAndLevels = teamNamesAndLevels ?? new TeamNameAndLevel[0];
            HasV1 = hasV1;
            ClubId = clubId;
            SeasonId = seasonId;
        }

        public string Id { get; }

        public TeamNameAndLevel[] TeamNamesAndLevels { get; }

        public bool HasV1 { get; }

        public int ClubId { get; }

        public int SeasonId { get; }

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
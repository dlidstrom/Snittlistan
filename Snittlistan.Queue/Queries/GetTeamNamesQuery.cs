namespace Snittlistan.Queue.Queries
{
    public class GetTeamNamesQuery : IQuery<GetTeamNamesQuery, GetTeamNamesQuery.Result>
    {
        public class Result
        {
            public Result(TeamNameAndLevel[] teamNameAndLevels)
            {
                TeamNameAndLevels = teamNameAndLevels;
            }

            public TeamNameAndLevel[] TeamNameAndLevels { get; }
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
        }
    }
}

namespace Snittlistan.Web.Areas.V1.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Raven.Imports.Newtonsoft.Json;

    /// <summary>
    /// Represents a league match.
    /// </summary>
    public class Match4x4
    {
        [JsonProperty(PropertyName = "Teams")]
        private readonly List<Team4x4> teams;

        /// <summary>
        /// Initializes a new instance of the Match4x4 class.
        /// </summary>
        /// <param name="location">Match location.</param>
        /// <param name="date">Match date.</param>
        /// <param name="teams">Teams that played the match.</param>
        [JsonConstructor]
        public Match4x4(
            string location,
            DateTimeOffset date,
            IEnumerable<Team4x4> teams)
        {
            Location = location;
            Date = date;
            this.teams = teams.ToList();
        }

        /// <summary>
        /// Initializes a new instance of the Match4x4 class.
        /// </summary>
        /// <param name="location">Match location.</param>
        /// <param name="date">Match date.</param>
        /// <param name="homeTeam">Home team.</param>
        /// <param name="awayTeam">Away team.</param>
        public Match4x4(
            string location,
            DateTimeOffset date,
            Team4x4 homeTeam,
            Team4x4 awayTeam)
        {
            Location = location;
            Date = date;
            teams = new List<Team4x4> { homeTeam, awayTeam };
        }

        /// <summary>
        /// Gets or sets the match id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the match location.
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// Gets or sets the match date.
        /// </summary>
        public DateTimeOffset Date { get; set; }

        /// <summary>
        /// Gets or sets the home team.
        /// </summary>
        [JsonIgnore]
        public Team4x4 HomeTeam
        {
            get { return teams[0]; }
            set { teams[0] = value; }
        }

        /// <summary>
        /// Gets or sets the away team.
        /// </summary>
        [JsonIgnore]
        public Team4x4 AwayTeam
        {
            get { return teams[1]; }
            set { teams[1] = value; }
        }

        /// <summary>
        /// Gets the teams.
        /// </summary>
        public IEnumerable<Team4x4> Teams
        {
            get { return teams; }
        }
    }
}
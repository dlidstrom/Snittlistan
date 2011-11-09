namespace Snittlistan.Models
{
    using System;
    using System.Collections.Generic;
    using Newtonsoft.Json;

    /// <summary>
    /// Represents a league match.
    /// </summary>
    public class Match
    {
        [JsonProperty(PropertyName = "Teams")]
        private List<Team> teams;

        /// <summary>
        /// Initializes a new instance of the Match class.
        /// </summary>
        /// <param name="location">Match location.</param>
        /// <param name="date">Match date.</param>
        /// <param name="bitsMatchId">BITS match id.</param>
        /// <param name="teams">Teams that played the match.</param>
        [JsonConstructor]
        public Match(
            string location,
            DateTime date,
            int bitsMatchId,
            IEnumerable<Team> teams)
        {
            Location = location;
            Date = date;
            BitsMatchId = bitsMatchId;
            this.teams = new List<Team>(teams);
        }

        /// <summary>
        /// Initializes a new instance of the Match class.
        /// </summary>
        /// <param name="location">Match location.</param>
        /// <param name="date">Match date.</param>
        /// <param name="bitsMatchId">BITS match id.</param>
        /// <param name="homeTeam">Home team.</param>
        /// <param name="awayTeam">Away team.</param>
        public Match(
            string location,
            DateTime date,
            int bitsMatchId,
            Team homeTeam,
            Team awayTeam)
        {
            Location = location;
            Date = date;
            BitsMatchId = bitsMatchId;
            teams = new List<Team> { homeTeam, awayTeam };
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
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the home team.
        /// </summary>
        [JsonIgnore]
        public Team HomeTeam
        {
            get { return teams[0]; }
            set { teams[0] = value; }
        }

        /// <summary>
        /// Gets or sets the away team.
        /// </summary>
        [JsonIgnore]
        public Team AwayTeam
        {
            get { return teams[1]; }
            set { teams[1] = value; }
        }

        /// <summary>
        /// Gets or sets the BITS match id.
        /// </summary>
        public int BitsMatchId { get; set; }

        /// <summary>
        /// Gets the teams.
        /// </summary>
        public IEnumerable<Team> Teams
        {
            get { return teams; }
        }
    }
}
namespace Snittlistan.Models
{
    using System.Collections.Generic;
    using System.Linq;
    using Newtonsoft.Json;

    /// <summary>
    /// Represents a serie in a match.
    /// </summary>
    public class Serie
    {
        [JsonProperty(PropertyName = "Tables")]
        private List<Table> tables;

        /// <summary>
        /// Initializes a new instance of the Serie class.
        /// </summary>
        /// <param name="tables">Tables of the serie.</param>
        public Serie(IEnumerable<Table> tables)
        {
            this.tables = new List<Table>(tables);
        }

        /// <summary>
        /// Gets the tables.
        /// </summary>
        public IEnumerable<Table> Tables
        {
            get { return tables; }
        }

        /// <summary>
        /// Returns the total pins for this serie.
        /// </summary>
        /// <returns>Total pins.</returns>
        public int Pins()
        {
            return Tables.Sum(t => t.Pins());
        }

        /// <summary>
        /// Gets the total score of this serie.
        /// </summary>
        /// <returns></returns>
        public int Score()
        {
            return Tables.Sum(t => t.Score);
        }

        /// <summary>
        /// Returns the pins for a player.
        /// </summary>
        /// <param name="player">Player name.</param>
        /// <returns>Pins for player.</returns>
        public int PinsForPlayer(string player)
        {
            return Tables.Sum(t => t.PinsForPlayer(player));
        }
    }
}
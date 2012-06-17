namespace Snittlistan.Models
{
    using System.Collections.Generic;
    using System.Linq;
    using Raven.Imports.Newtonsoft.Json;

    /// <summary>
    /// Represents a serie in a match.
    /// </summary>
    public class Serie8x4
    {
        [JsonProperty(PropertyName = "Tables")]
        private readonly List<Table8x4> tables;

        /// <summary>
        /// Initializes a new instance of the Serie8x4 class.
        /// </summary>
        /// <param name="tables">Tables of the serie.</param>
        public Serie8x4(IEnumerable<Table8x4> tables)
        {
            this.tables = tables.ToList();
        }

        /// <summary>
        /// Gets the tables.
        /// </summary>
        public IEnumerable<Table8x4> Tables
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
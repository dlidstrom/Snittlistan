using System;
using EventStoreLite;
using Raven.Imports.Newtonsoft.Json;
using Snittlistan.Web.Areas.V2.Domain;

namespace Snittlistan.Web.Areas.V2.ReadModels
{
    // TODO Store only roster id. Use index to read and display roster values
    public class ResultHeaderReadModel : IReadModel
    {
        public ResultHeaderReadModel(
            Roster roster,
            string aggregateId,
            int teamScore,
            int opponentScore,
            int bitsMatchId,
            bool isFourPlayer)
        {
            Id = IdFromBitsMatchId(bitsMatchId);
            SetValues(roster, aggregateId, teamScore, opponentScore, bitsMatchId, isFourPlayer);
        }

        [JsonConstructor]
        private ResultHeaderReadModel()
        {
        }

        public string AggregateId { get; private set; }

        public int BitsMatchId { get; private set; }

        public DateTimeOffset Date { get; private set; }

        /// <summary>
        /// The BITS match id.
        /// </summary>
        public string Id { get; private set; }

        public string Location { get; private set; }

        public string Opponent { get; private set; }

        public int OpponentScore { get; private set; }

        public string RosterId { get; private set; }

        public int Season { get; private set; }

        public string Team { get; private set; }

        public string TeamLevel { get; private set; }

        public int TeamScore { get; private set; }

        public int Turn { get; private set; }

        public bool IsFourPlayer { get; private set; }

        public static string IdFromBitsMatchId(int bitsMatchId)
        {
            return "ResultHeader-" + bitsMatchId;
        }

        public void SetValues(Roster roster, string aggregateId, int teamScore, int opponentScore, int bitsMatchId, bool isFourPlayer)
        {
            Season = roster.Season;
            Turn = roster.Turn;
            AggregateId = aggregateId;
            RosterId = roster.Id;
            Date = roster.Date;
            Location = roster.Location;
            TeamLevel = roster.TeamLevel;
            Team = roster.Team;
            Opponent = roster.Opponent;
            TeamScore = teamScore;
            OpponentScore = opponentScore;
            BitsMatchId = bitsMatchId;
            IsFourPlayer = isFourPlayer;
        }
    }
}
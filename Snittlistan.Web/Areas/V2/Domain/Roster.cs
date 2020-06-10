namespace Snittlistan.Web.Areas.V2.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Raven.Abstractions;

    public class Roster
    {
        private string teamLevel;
        private List<string> acceptedPlayers;
        private List<string> players;

        public Roster(
            int season,
            int turn,
            int bitsMatchId,
            string team,
            string teamLevel,
            string location,
            string opponent,
            DateTime date,
            bool isFourPlayer,
            OilPatternInformation oilPattern)
        {
            Season = season;
            Turn = turn;
            BitsMatchId = bitsMatchId;
            Team = team;
            TeamLevel = teamLevel ?? Team.Substring(team.Length - 1);
            Location = location;
            Opponent = opponent;
            Date = date;
            IsFourPlayer = isFourPlayer;
            OilPattern = oilPattern ?? new OilPatternInformation(string.Empty, string.Empty);
            Players = new List<string>();
        }

        public string Id { get; set; }

        public int Season { get; set; }

        public int Turn { get; set; }

        public int BitsMatchId { get; set; }

        public string Team { get; set; }

        public string TeamLevel
        {
            get { return teamLevel; }
            set { teamLevel = value.Trim(); }
        }

        public string Location { get; set; }

        public string Opponent { get; set; }

        public DateTime Date { get; set; }

        public bool IsFourPlayer { get; set; }

        public OilPatternInformation OilPattern { get; set; }

        public bool Preliminary { get; set; }

        public List<string> Players
        {
            get => players;
            set
            {
                players = value;
                AcceptedPlayers.RemoveAll(x => players.Contains(x) == false);
            }
        }

        public string MatchResultId { get; set; }

        public string TeamLeader { get; set; }

        public bool IsVerified { get; set; }

        public List<string> AcceptedPlayers
        {
            get { return acceptedPlayers ?? new List<string>(); }
            set { acceptedPlayers = value; }
        }

        public void Accept(string playerId)
        {
            if (playerId == null) throw new ArgumentNullException(nameof(playerId));
            if (Preliminary) throw new Exception("Can not accept when preliminary");
            if (SystemTime.UtcNow > Date.ToUniversalTime()) throw new Exception("Can not accept passed games");
            if (Players.Contains(playerId) == false) throw new Exception("Can only accept players on the roster");
            AcceptedPlayers = new HashSet<string>(AcceptedPlayers.Concat(new[] { playerId })).ToList();
        }
    }
}
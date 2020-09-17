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
            OilPatternInformation oilPattern,
            List<AuditLogEntry> auditLogEntries = null)
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
            AuditLogEntries = auditLogEntries ?? new List<AuditLogEntry>();
        }

        public string Id { get; set; }

        public int Season { get; set; }

        public int Turn { get; set; }

        public int BitsMatchId { get; set; }

        public string Team { get; set; }

        // TODO: should this be calculated from Team?
        // Team.Substring(roster.Team.LastIndexOf(' ') + 1)
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
        public List<AuditLogEntry> AuditLogEntries { get; }
        public bool Preliminary { get; set; }

        public List<string> Players { get; set; } = new List<string>();

        public void SetPlayers(List<string> players, string userId)
        {
            Players = players;
            AcceptedPlayers.RemoveAll(x => players.Contains(x) == false);
        }

        public string MatchResultId { get; set; }

        public string TeamLeader { get; set; }

        public bool IsVerified { get; set; }

        public List<string> AcceptedPlayers
        {
            get { return acceptedPlayers ?? new List<string>(); }
            set { acceptedPlayers = value; }
        }

        public void Update(Change change)
        {
            object before = GetState();

            if (change.PlayerAccepted != null)
            {
                Accept(change.PlayerAccepted.NewValue);
            }

            object after = GetState();
            var auditLogEntry = new AuditLogEntry(
                change.UserId,
                change.ChangeType.ToString(),
                change,
                before,
                after);
            AuditLogEntries.Add(auditLogEntry);
        }

        private void Accept(string playerId)
        {
            if (playerId == null) throw new ArgumentNullException(nameof(playerId));
            if (Preliminary) throw new Exception("Can not accept when preliminary");
            if (SystemTime.UtcNow > Date.ToUniversalTime()) throw new Exception("Can not accept passed games");
            if (Players.Contains(playerId) == false) throw new Exception("Can only accept players on the roster");
            AcceptedPlayers = new HashSet<string>(AcceptedPlayers.Concat(new[] { playerId })).ToList();
        }

        private object GetState()
        {
            return new
            {
                Players = Players.ToArray(),
                AcceptedPlayers = AcceptedPlayers.ToArray()
            };
        }

        public class Change
        {
            public Change(
                ChangeType changeType,
                string userId,
#if DEBUG
                int _ = 0,
#endif
                AuditLogEntry.PropertyChange<string> playerAccepted = null
                )
            {
                ChangeType = changeType;
                UserId = userId;
                PlayerAccepted = playerAccepted;
            }

            public ChangeType ChangeType { get; }
            public string UserId { get; }
            public AuditLogEntry.PropertyChange<string> PlayerAccepted { get; }
        }

        public enum ChangeType
        {
            PlayerAccepted
        }
    }
}

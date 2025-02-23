﻿#nullable enable

using Raven.Abstractions;
using Raven.Client;

namespace Snittlistan.Web.Areas.V2.Domain;

public class Roster : IAuditLogCapable
{
    private string? teamLevel;
    private List<string>? acceptedPlayers;

    public Roster(
        int season,
        int turn,
        int bitsMatchId,
        string team,
        string? teamLevel,
        string? location,
        string? opponent,
        DateTime date,
        bool isFourPlayer,
        OilPatternInformation oilPattern,
        List<AuditLogEntry>? auditLogEntries = null)
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

    public string Id { get; set; } = null!;

    public int Season { get; set; }

    public int Turn { get; set; }

    public int BitsMatchId { get; set; }

    public string Team { get; set; }

    // TODO: should this be calculated from Team?
    // Team.Substring(roster.Team.LastIndexOf(' ') + 1)
    public string? TeamLevel
    {
        get => teamLevel;

        set => teamLevel = value!.Trim();
    }

    public string? Location { get; set; }

    public string? Opponent { get; set; }

    public DateTime Date { get; set; }

    public bool IsFourPlayer { get; set; }

    public OilPatternInformation OilPattern { get; set; }
    public List<AuditLogEntry> AuditLogEntries { get; }
    public int Version => AuditLogEntries.Count;
    public bool Preliminary { get; set; }
    public bool ManuallyAdded { get; set; }

    public List<string> Players { get; set; } = new List<string>();

    public void SetPlayers(List<string> players)
    {
        Players = players;
        _ = AcceptedPlayers.RemoveAll(x => players.Contains(x) == false);
    }

    public string? MatchResultId { get; set; }

    public string? TeamLeader { get; set; }

    public bool IsVerified { get; set; }

    public List<string> AcceptedPlayers
    {
        get => acceptedPlayers ?? new List<string>();

        set => acceptedPlayers = value;
    }

    public bool SkipRegistration { get; set; }

    public bool MatchTimeChanged { get; set; }

    public AuditLogEntry? UpdateWith(Guid correlationId, Update update)
    {
        Change change = new(update.ChangeType, update.UserId);
        RosterState before = GetState();

        update.PlayerAccepted.Match(
            x =>
            {
                change.PlayerAccepted = AuditLogEntry.PropertyChange.Create(null, x);
                Accept(x);
            },
            () => { });

        update.OilPattern.Match(
            x =>
            {
                if (x != OilPattern)
                {
                    change.OilPattern = AuditLogEntry.PropertyChange.Create(OilPattern, x);
                    OilPattern = x;
                }
            },
            () => { });

        update.Date.Match(
            x =>
            {
                if (x != Date)
                {
                    change.Date = AuditLogEntry.PropertyChange.Create(Date, x);
                    Date = x;
                }
            },
            () => { });

        update.Opponent.Match(
            x =>
            {
                if (x != Opponent)
                {
                    change.Opponent = AuditLogEntry.PropertyChange.Create(Opponent, x);
                    Opponent = x;
                }
            },
            () => { });

        update.Location.Match(
            x =>
            {
                if (x != Location)
                {
                    change.Location = AuditLogEntry.PropertyChange.Create(Location, x);
                    Location = x;
                }
            },
            () => { });

        update.Preliminary.Match(
            x =>
            {
                if (x != Preliminary)
                {
                    change.Preliminary = AuditLogEntry.PropertyChange.Create(Preliminary, x);
                    Preliminary = x;
                }
            },
            () => { });

        update.Players.Match(
            x =>
            {
                if (x.SequenceEqual(Players) == false)
                {
                    change.Players = AuditLogEntry.PropertyChange.Create(Players, x);
                    Players = x;
                }
            },
            () => { });

        update.IsVerified.Match(
            x =>
            {
                if (x != IsVerified)
                {
                    change.IsVerified = AuditLogEntry.PropertyChange.Create(IsVerified, x);
                    IsVerified = x;
                }
            },
            () => { });

        update.TeamLeader.Match(
            x =>
            {
                if (x != TeamLeader)
                {
                    change.TeamLeader = AuditLogEntry.PropertyChange.Create(TeamLeader, x);
                    TeamLeader = x;
                }
            },
            () => { });

        AuditLogEntry? auditLogEntry = null;
        if (change.HasChanges)
        {
            RosterState after = GetState();
            auditLogEntry = new(
                change.UserId,
                change.ChangeType.ToString(),
                correlationId == default ? Guid.NewGuid() : correlationId,
                change,
                before,
                after);
            AuditLogEntries.Add(auditLogEntry);
        }

        return auditLogEntry;
    }

    private void Accept(string playerId)
    {
        if (playerId == null)
        {
            throw new ArgumentNullException(nameof(playerId));
        }

        if (Preliminary)
        {
            throw new Exception("Can not accept when preliminary");
        }

        if (SystemTime.UtcNow > Date.ToUniversalTime())
        {
            throw new Exception("Can not accept passed games");
        }

        if (Players.Contains(playerId) == false)
        {
            throw new Exception("Can only accept players on the roster");
        }

        AcceptedPlayers = new HashSet<string>(AcceptedPlayers.Concat(new[] { playerId })).ToList();
    }

    private RosterState GetState()
    {
        return new RosterState(Players.ToArray(), AcceptedPlayers.ToArray(), TeamLeader);
    }

    public FormattedAuditLog GetFormattedAuditLog(IDocumentSession documentSession, Guid correlationId)
    {
        return DoGetAuditLog(documentSession, AuditLogEntries.Where(x => x.CorrelationId == correlationId));
    }

    public FormattedAuditLog GetFormattedAuditLog(IDocumentSession documentSession)
    {
        return DoGetAuditLog(documentSession, AuditLogEntries);
    }

    private FormattedAuditLog DoGetAuditLog(IDocumentSession documentSession, IEnumerable<AuditLogEntry> auditLogEntries)
    {
        return new FormattedAuditLog(
            $"{Team} - {Opponent}, Omgång {Turn}, {Date:yyyy-MM-dd HH:mm}",
            auditLogEntries.Select(Format).ToArray());

        FormattedAuditLogEntry Format(AuditLogEntry entry)
        {
            Change change = (Change)entry.Change;
            string? action = null;
            switch (change.ChangeType)
            {
                case ChangeType.PlayerAccepted:
                    {
                        Player player = documentSession.Load<Player>(change.PlayerAccepted?.NewValue);
                        action = $"{player.Name} accepterade";
                        break;
                    }
                case ChangeType.VerifyMatchMessage:
                    {
                        action = "Verifierade resultatet";
                        break;
                    }
                case ChangeType.EditPlayers:
                    {
                        List<string> changes = new();
                        if (change.Preliminary is not null)
                        {
                            string text =
                                change.Preliminary.NewValue
                                ? "Gjordes preliminär"
                                : "Inte längre preliminär";
                            changes.Add(text);
                        }

                        if (change.Players is not null)
                        {
                            string?[] outOfTeam = change.Players.OldValue
                                .Except(change.Players.NewValue)
                                .Select(PlayerName)
                                .ToArray();
                            string?[] intoTeam = change.Players.NewValue
                                .Except(change.Players.OldValue)
                                .Select(PlayerName)
                                .ToArray();
                            if (outOfTeam.Length == 0)
                            {
                                if (intoTeam.Length is 1 or 2)
                                {
                                    changes.Add($"Tog ut reserv {string.Join(", ", intoTeam)}");
                                }
                                else if (outOfTeam.Length == intoTeam.Length)
                                {
                                    changes.Add("Ändrade ordningen i laget");
                                }
                                else
                                {
                                    changes.Add($"Tog ut lag {string.Join(", ", intoTeam)}");
                                }
                            }
                            else if ((outOfTeam.Length == 1 || outOfTeam.Length == 2)
                                && intoTeam.Length == 0)
                            {
                                changes.Add($"Tog bort reserv {string.Join(", ", outOfTeam)}");
                            }
                            else
                            {
                                string replaced = string.Join(", ", outOfTeam);
                                string inserted = string.Join(", ", intoTeam);
                                changes.Add($"Ändrade spelare ({replaced} byttes mot {inserted})");
                            }
                        }

                        if (change.TeamLeader is not null)
                        {
                            if (change.TeamLeader.NewValue is not null)
                            {
                                string leader = PlayerName(change.TeamLeader.NewValue);
                                changes.Add($"Valde {leader} till lagledare");
                            }
                            else
                            {
                                string replaced = PlayerName(change.TeamLeader?.OldValue);
                                changes.Add($"Tog bort {replaced} som lagledare");
                            }
                        }

                        action = string.Join("; ", changes);
                        break;
                    }

                    string PlayerName(string? playerId)
                    {
                        Player player = documentSession.Load<Player>(playerId);
                        return player?.Nickname ?? player?.Name ?? "?";
                    }

                default:
                    break;
            }

            return new FormattedAuditLogEntry(
                GetUser(entry.UserId),
                action,
                entry.Date);

            string GetUser(string userId)
            {
                Player player = documentSession.Load<Player>(entry.UserId);
                return player?.Name ?? userId;
            }
        }
    }

    public class Change
    {
        public Change(ChangeType changeType, string userId)
        {
            ChangeType = changeType;
            UserId = userId;
        }

        public ChangeType ChangeType { get; }

        public string UserId { get; }

        public bool HasChanges =>
            PlayerAccepted is not null
            || OilPattern is not null
            || Date is not null
            || Opponent is not null
            || Location is not null
            || Players is not null
            || IsVerified is not null
            || Preliminary is not null
            || TeamLeader is not null;

        public AuditLogEntry.PropertyChange<string?>? PlayerAccepted { get; set; }
        public AuditLogEntry.PropertyChange<OilPatternInformation?>? OilPattern { get; set; }
        public AuditLogEntry.PropertyChange<DateTime>? Date { get; set; }
        public AuditLogEntry.PropertyChange<string?>? Opponent { get; set; }
        public AuditLogEntry.PropertyChange<string?>? Location { get; set; }
        public AuditLogEntry.PropertyChange<List<string>?>? Players { get; set; }
        public AuditLogEntry.PropertyChange<bool>? IsVerified { get; set; }
        public AuditLogEntry.PropertyChange<bool>? Preliminary { get; set; }
        public AuditLogEntry.PropertyChange<string?>? TeamLeader { get; set; }
    }

    public class Update
    {
        public Update(
            ChangeType changeType,
            string userId)
        {
            ChangeType = changeType;
            UserId = userId;
        }

        public ChangeType ChangeType { get; }
        public string UserId { get; }
        public Option<string> PlayerAccepted { get; set; } = None.Value;
        public Option<OilPatternInformation> OilPattern { get; set; } = None.Value;
        public Option<DateTime> Date { get; set; } = None.Value;
        public Option<string> Opponent { get; set; } = None.Value;
        public Option<string> Location { get; set; } = None.Value;
        public Option<bool> Preliminary { get; set; } = None.Value;
        public Option<List<string>> Players { get; set; } = None.Value;
        public Option<bool> IsVerified { get; set; } = None.Value;
        public Option<string?> TeamLeader { get; set; } = None.Value;
    }

    public enum ChangeType
    {
        PlayerAccepted,
        VerifyMatchMessage,
        EditPlayers
    }
}

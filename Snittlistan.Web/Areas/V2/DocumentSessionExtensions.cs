namespace Snittlistan.Web.Areas.V2
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Raven.Abstractions;
    using Raven.Client;
    using Snittlistan.Web.Areas.V2.Domain;
    using Snittlistan.Web.Areas.V2.Indexes;
    using ViewModels;

    public static class DocumentSessionExtensions
    {
        public static SelectListItem[] CreateRosterSelectList(
            this IDocumentSession session,
            int season,
            string rosterId = "",
            Func<Roster, bool> pred = null)
        {
            return GetRosterSelectList(session, season, rosterId, false, pred ?? (x => true));
        }

        public static RosterViewModel LoadRosterViewModel(
            this IDocumentSession session,
            Roster roster)
        {
            var accepted = new HashSet<string>(roster.AcceptedPlayers);
            var players = new List<Tuple<string, string, bool>>();
            foreach (var player in roster.Players.Where(p => p != null).Select(session.Load<Player>))
            {
                players.Add(Tuple.Create(player.Id, player.Name, accepted.Contains(player.Id)));
            }

            Tuple<string, string, bool> teamLeaderTuple = null;
            if (roster.TeamLeader != null)
            {
                var teamLeader = session.Load<Player>(roster.TeamLeader);
                teamLeaderTuple =
                    Tuple.Create(
                        teamLeader.Id,
                        teamLeader.Name,
                        accepted.Contains(teamLeader.Id));
            }

            var vm = new RosterViewModel(roster, teamLeaderTuple, players);
            return vm;
        }

        public static SelectListItem[] CreatePlayerSelectList(
            this IDocumentSession documentSession, string player = "", Func<Player[]> getPlayers = null, Func<Player, string> textFormatter = null)
        {
            var playerList = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Text = "Välj medlem",
                    Value = string.Empty
                }
            };
            var query =
                getPlayers != null
                    ? getPlayers.Invoke()
                    : documentSession.Query<Player, PlayerSearch>()
                                     .Where(x => x.PlayerStatus == Player.Status.Active)
                                     .ToArray();

            var players = query.OrderBy(x => x.Name)
                               .Select(x => new SelectListItem
                               {
                                   Text = textFormatter != null ? textFormatter.Invoke(x) : x.Name,
                                   Value = x.Id,
                                   Selected = x.Id == player
                               })
                               .ToArray();
            playerList.AddRange(players);
            return playerList.ToArray();
        }

        private static SelectListItem[] GetRosterSelectList(
            IDocumentSession session,
            int season,
            string rosterId,
            bool bits,
            Func<Roster, bool> pred)
        {
            var query = session.Query<Roster, RosterSearchTerms>()
                               .Where(x => x.Season == season)
                               .Where(x => x.Date < SystemTime.UtcNow)
                               .Where(x => x.Preliminary == false);
            if (bits)
            {
                query = query.Where(x => x.BitsMatchId != 0);
            }
            else
            {
                query = query.Where(x => x.BitsMatchId == 0);
            }

            var rosterSelectList = query.OrderBy(x => x.Date)
                                        .ToList()
                                        .Where(x => x.MatchResultId == null || string.IsNullOrEmpty(rosterId) == false)
                                        .Where(pred)
                                        .Select(
                                            x => new SelectListItem
                                            {
                                                Text = $"{x.Turn}: {x.Team} - {x.Opponent} ({x.Location} {x.Date.ToShortTimeString()})",
                                                Value = x.Id,
                                                Selected = x.Id == rosterId
                                            })
                                        .ToArray();
            return rosterSelectList;
        }
    }
}
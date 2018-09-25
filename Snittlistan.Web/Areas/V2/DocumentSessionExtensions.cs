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
        public static SelectListItem[] CreateBitsRosterSelectList(
            this IDocumentSession session,
            int season,
            string rosterId = "")
        {
            return GetRosterSelectList(session, season, rosterId, true);
        }

        public static SelectListItem[] CreateRosterSelectList(
            this IDocumentSession session,
            int season,
            string rosterId = "")
        {
            return GetRosterSelectList(session, season, rosterId, false);
        }

        public static RosterViewModel LoadRosterViewModel(
            this IDocumentSession session,
            Roster roster)
        {
            var players = new List<Tuple<string, string>>();
            foreach (var player in roster.Players.Where(p => p != null).Select(session.Load<Player>))
            {
                players.Add(Tuple.Create(player.Id, player.Name));
            }

            Tuple<string, string> teamLeaderTuple = null;
            if (roster.TeamLeader != null)
            {
                var teamLeader = session.Load<Player>(roster.TeamLeader);
                teamLeaderTuple = Tuple.Create(teamLeader.Id, teamLeader.Name);
            }

            var vm = new RosterViewModel(roster, teamLeaderTuple, players);
            return vm;
        }

        private static SelectListItem[] GetRosterSelectList(
            IDocumentSession session,
            int season,
            string rosterId,
            bool bits)
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
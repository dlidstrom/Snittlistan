#nullable enable

using Raven.Client;
using Snittlistan.Queue.Messages;
using Snittlistan.Web.Areas.V2;
using Snittlistan.Web.Areas.V2.Domain;
using Snittlistan.Web.Areas.V2.Indexes;
using Snittlistan.Web.Infrastructure;
using Snittlistan.Web.Infrastructure.Bits.Contracts;
using Snittlistan.Web.Models;

namespace Snittlistan.Web.TaskHandlers;

public class GetRostersFromBitsTaskHandler : TaskHandler<GetRostersFromBitsTask>
{
    public override async Task Handle(MessageContext<GetRostersFromBitsTask> task)
    {
        WebsiteConfig websiteConfig = CompositionRoot.DocumentSession.Load<WebsiteConfig>(WebsiteConfig.GlobalId);
        Log.Info($"Importing BITS season {websiteConfig.SeasonId} for {task.Tenant.TeamFullName} (ClubId={websiteConfig.ClubId})");
        RosterSearchTerms.Result[] rosterSearchTerms =
            CompositionRoot.DocumentSession.Query<RosterSearchTerms.Result, RosterSearchTerms>()
                .Where(x => x.Season == websiteConfig.SeasonId)
                .Where(x => x.BitsMatchId != 0)
                .ProjectFromIndexFieldsInto<RosterSearchTerms.Result>()
                .ToArray();
        Roster[] rosters = CompositionRoot.DocumentSession.Load<Roster>(rosterSearchTerms.Select(x => x.Id));
        HashSet<int> foundMatchIds = new();

        // Team
        Log.Info($"Fetching teams");
        TeamResult[] teams = await CompositionRoot.BitsClient.GetTeam(websiteConfig.ClubId, websiteConfig.SeasonId);
        foreach (TeamResult teamResult in teams)
        {
            // Division
            Log.Info($"Fetching divisions");
            DivisionResult[] divisionResults = await CompositionRoot.BitsClient.GetDivisions(teamResult.TeamId, websiteConfig.SeasonId);

            // Match
            if (divisionResults.Length != 1)
            {
                throw new Exception($"Unexpected number of divisions: {divisionResults.Length}");
            }

            DivisionResult divisionResult = divisionResults[0];
            Log.Info($"Fetching match rounds");
            MatchRound[] matchRounds = await CompositionRoot.BitsClient.GetMatchRounds(teamResult.TeamId, divisionResult.DivisionId, websiteConfig.SeasonId);
            Dictionary<int, MatchRound> dict = matchRounds.ToDictionary(x => x.MatchId);
            foreach (int key in dict.Keys)
            {
                _ = foundMatchIds.Add(key);
            }

            // update existing rosters
            foreach (Roster roster in rosters.Where(x => dict.ContainsKey(x.BitsMatchId)))
            {
                Log.Info($"Updating roster {roster.Id}");
                MatchRound matchRound = dict[roster.BitsMatchId];
                roster.OilPattern = OilPatternInformation.Create(
                    matchRound.MatchOilPatternName!,
                    matchRound.MatchOilPatternId);
                roster.Date = matchRound.MatchDate!.ToDateTime(matchRound.MatchTime);
                roster.Turn = matchRound.MatchRoundId;
                roster.MatchTimeChanged = matchRound.MatchStatus == 2;
                if (matchRound.HomeTeamClubId == websiteConfig.ClubId)
                {
                    roster.Team = matchRound.MatchHomeTeamAlias!;
                    roster.TeamLevel = roster.Team.Substring(roster.Team.LastIndexOf(' ') + 1);
                    roster.Opponent = matchRound.MatchAwayTeamAlias!;
                }
                else if (matchRound.AwayTeamClubId == websiteConfig.ClubId)
                {
                    roster.Team = matchRound.MatchAwayTeamAlias!;
                    roster.TeamLevel = roster.Team.Substring(roster.Team.LastIndexOf(' ') + 1);
                    roster.Opponent = matchRound.MatchHomeTeamAlias!;
                }
                else
                {
                    throw new Exception($"Unknown clubs: {matchRound.HomeTeamClubId} {matchRound.AwayTeamClubId}");
                }

                roster.Location = matchRound.MatchHallName!;
            }

            // add missing rosters
            HashSet<int> existingMatchIds = new(rosters.Select(x => x.BitsMatchId));
            foreach (int matchId in dict.Keys.Where(x => existingMatchIds.Contains(x) == false))
            {
                Log.Info($"Adding match {matchId}");
                MatchRound matchRound = dict[matchId];
                string team;
                string opponent;
                if (matchRound.HomeTeamClubId == websiteConfig.ClubId)
                {
                    team = matchRound.MatchHomeTeamAlias!;
                    opponent = matchRound.MatchAwayTeamAlias!;
                }
                else if (matchRound.AwayTeamClubId == websiteConfig.ClubId)
                {
                    team = matchRound.MatchAwayTeamAlias!;
                    opponent = matchRound.MatchHomeTeamAlias!;
                }
                else
                {
                    throw new Exception($"Unknown clubs: {matchRound.HomeTeamClubId} {matchRound.AwayTeamClubId}");
                }

                Roster roster = new(
                    matchRound.MatchSeason,
                    matchRound.MatchRoundId,
                    matchRound.MatchId,
                    team!,
                    team.Substring(team.LastIndexOf(' ') + 1),
                    matchRound.MatchHallName!,
                    opponent,
                    matchRound.MatchDate!.ToDateTime(matchRound.MatchTime),
                    matchRound.MatchNbrOfPlayers == 4,
                    OilPatternInformation.Create(matchRound.MatchOilPatternName!, matchRound.MatchOilPatternId))
                {
                    MatchTimeChanged = matchRound.MatchStatus == 2
                };
                CompositionRoot.DocumentSession.Store(roster);
            }
        }

        // remove extraneous rosters
        Roster[] toRemove = rosters.Where(x => foundMatchIds.Contains(x.BitsMatchId) == false).ToArray();
        if (toRemove.Any())
        {
            string body = $"Rosters to remove: {string.Join(",", toRemove.Select(x => $"Id={x.Id} BitsMatchId={x.BitsMatchId}"))}";
            Log.Info(body);
            foreach (Roster roster in toRemove)
            {
                CompositionRoot.DocumentSession.Delete(roster);
            }

            SendEmail email = SendEmail.ToAdmin(
                $"Removed rosters for {task.Tenant.TeamFullName}",
                body);
            await CompositionRoot.EmailService.SendAsync(email);
        }
    }
}

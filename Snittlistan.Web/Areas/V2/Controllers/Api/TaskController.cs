namespace Snittlistan.Web.Areas.V2.Controllers.Api
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web.Http;
    using Elmah;
    using Infrastructure.Bits;
    using Newtonsoft.Json;
    using NLog;
    using Raven.Abstractions;
    using Raven.Client;
    using Snittlistan.Queue.Messages;
    using Snittlistan.Web.Areas.V2.Commands;
    using Snittlistan.Web.Areas.V2.Domain;
    using Snittlistan.Web.Areas.V2.Domain.Match;
    using Snittlistan.Web.Areas.V2.Indexes;
    using Snittlistan.Web.Areas.V2.Queries;
    using Snittlistan.Web.Areas.V2.ReadModels;
    using Snittlistan.Web.Controllers;
    using Snittlistan.Web.Helpers;
    using Snittlistan.Web.Infrastructure.Attributes;
    using Snittlistan.Web.Infrastructure.Indexes;
    using Snittlistan.Web.Models;

    [OnlyLocalAllowed]
    public class TaskController : AbstractApiController
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private readonly IBitsClient bitsClient;

        public TaskController(IBitsClient bitsClient)
        {
            this.bitsClient = bitsClient;
        }

        public async Task<IHttpActionResult> Post(TaskRequest request)
        {
            Log.Info("Received task");
            var result = await HandleTask(request);
            Log.Info("Done");
            return result;
        }

        private async Task<IHttpActionResult> HandleTask(TaskRequest request)
        {
            if (ModelState.IsValid == false) return BadRequest(ModelState);
            var taskObject = JsonConvert.DeserializeObject(
                request.TaskJson,
                new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All });
            switch (taskObject)
            {
                case OneTimeKeyEvent @event:
                    return Handle(@event);
                case VerifyMatchMessage message:
                    return await Handle(message);
                case RegisterMatchesMessage message:
                    return await Handle(message);
                case InitializeIndexesMessage message:
                    return Handle(message);
                case VerifyMatchesMessage message:
                    return Handle(message);
                case NewUserCreatedEvent @event:
                    return Handle(@event);
                case UserInvitedEvent @event:
                    return Handle(@event);
                case EmailTask task:
                    return Handle(task);
                case MatchRegisteredEvent @event:
                    return Handle(@event);
                case GetRostersFromBitsMessage message:
                    return await Handle(message);
                case GetPlayersFromBitsMessage message:
                    return await Handle(message);
            }

            throw new Exception($"Unhandled task {taskObject.GetType()}");
        }

        private async Task<IHttpActionResult> Handle(GetPlayersFromBitsMessage message)
        {
            var websiteConfig = DocumentSession.Load<WebsiteConfig>(WebsiteConfig.GlobalId);
            var playersResult = await bitsClient.GetPlayers(websiteConfig.ClubId);
            var players = DocumentSession.Query<Player, PlayerSearch>()
                                         .ToArray();

            // update existing players by matching on license number
            var playersByLicense = playersResult.Data.ToDictionary(x => x.LicNbr);
            foreach (var player in players.Where(x => x.PlayerItem != null))
            {
                if (playersByLicense.TryGetValue(player.PlayerItem.LicNbr, out var playerItem))
                {
                    player.PlayerItem = playerItem;
                    playersByLicense.Remove(player.PlayerItem.LicNbr);
                }
            }

            // add missing players, i.e. what is left from first step
            // try first to match on name, update those, add the rest
            var playerNamesWithoutPlayerItem = players.Where(x => x.PlayerItem == null).ToDictionary(x => x.Name);
            foreach (var playerItem in playersByLicense.Values)
            {
                // look for name
                var nameFromBits = $"{playerItem.FirstName} {playerItem.SurName}";
                if (playerNamesWithoutPlayerItem.TryGetValue(nameFromBits, out var player))
                {
                    player.PlayerItem = playerItem;
                }
                else
                {
                    // create new
                    var personalNumber = int.Parse(playerItem.LicNbr.Substring(5, 2)
                                                   + playerItem.LicNbr.Substring(3, 2)
                                                   + playerItem.LicNbr.Substring(1, 2));
                    var newPlayer = new Player(
                        $"{playerItem.FirstName} {playerItem.SurName}",
                        playerItem.Email,
                        playerItem.Inactive ? Player.Status.Inactive : Player.Status.Active,
                        personalNumber,
                        string.Empty,
                        new string[0])
                    {
                        PlayerItem = playerItem
                    };
                    DocumentSession.Store(newPlayer);
                }
            }

            return Ok();
        }

        private async Task<IHttpActionResult> Handle(GetRostersFromBitsMessage message)
        {
            var websiteConfig = DocumentSession.Load<WebsiteConfig>(WebsiteConfig.GlobalId);
            var rosterSearchTerms = DocumentSession.Query<RosterSearchTerms.Result, RosterSearchTerms>()
                                                   .Where(x => x.Season == websiteConfig.SeasonId)
                                                   .Where(x => x.BitsMatchId != 0)
                                                   .ProjectFromIndexFieldsInto<RosterSearchTerms.Result>()
                                                   .ToArray();
            var rosters = DocumentSession.Load<Roster>(rosterSearchTerms.Select(x => x.Id));

            // Team
            var teams = await bitsClient.GetTeam(websiteConfig.ClubId, websiteConfig.SeasonId);
            foreach (var teamResult in teams)
            {
                // Division
                var divisionResults = await bitsClient.GetDivisions(teamResult.TeamId, websiteConfig.SeasonId);

                // Match
                if (divisionResults.Length != 1) throw new Exception($"Unexpected number of divisions: {divisionResults.Length}");
                var divisionResult = divisionResults[0];
                var matchRounds = await bitsClient.GetMatchRounds(teamResult.TeamId, divisionResult.DivisionId, websiteConfig.SeasonId);
                var dict = matchRounds.ToDictionary(x => x.MatchId);

                // update existing rosters
                foreach (var roster in rosters.Where(x => dict.ContainsKey(x.BitsMatchId)))
                {
                    var matchRound = dict[roster.BitsMatchId];
                    roster.OilPattern = OilPatternInformation.Create(
                        matchRound.MatchOilPatternName,
                        matchRound.MatchOilPatternId);
                    roster.Date = matchRound.MatchDate.ToDateTime(matchRound.MatchTime);
                    if (matchRound.HomeTeamClubId == websiteConfig.ClubId)
                    {
                        roster.Team = matchRound.MatchHomeTeamAlias;
                        roster.TeamLevel = roster.Team.Substring(roster.Team.LastIndexOf(' ') + 1);
                        roster.Opponent = matchRound.MatchAwayTeamAlias;
                    }
                    else if (matchRound.AwayTeamClubId == websiteConfig.ClubId)
                    {
                        roster.Team = matchRound.MatchAwayTeamAlias;
                        roster.TeamLevel = roster.Team.Substring(roster.Team.LastIndexOf(' ') + 1);
                        roster.Opponent = matchRound.MatchHomeTeamAlias;
                    }
                    else
                    {
                        throw new Exception($"Unknown clubs: {matchRound.HomeTeamClubId} {matchRound.AwayTeamClubId}");
                    }

                    roster.Location = matchRound.MatchHallName;
                }

                // add missing rosters
                var existingMatchIds = new HashSet<int>(rosters.Select(x => x.BitsMatchId));
                foreach (var matchId in dict.Keys.Where(x => existingMatchIds.Contains(x) == false))
                {
                    var matchRound = dict[matchId];
                    string team;
                    string opponent;
                    if (matchRound.HomeTeamClubId == websiteConfig.ClubId)
                    {
                        team = matchRound.MatchHomeTeamAlias;
                        opponent = matchRound.MatchAwayTeamAlias;
                    }
                    else if (matchRound.AwayTeamClubId == websiteConfig.ClubId)
                    {
                        team = matchRound.MatchAwayTeamAlias;
                        opponent = matchRound.MatchHomeTeamAlias;
                    }
                    else
                    {
                        throw new Exception($"Unknown clubs: {matchRound.HomeTeamClubId} {matchRound.AwayTeamClubId}");
                    }

                    var roster = new Roster(
                        matchRound.MatchSeason,
                        matchRound.MatchRoundId,
                        matchRound.MatchId,
                        team,
                        team.Substring(team.LastIndexOf(' ') + 1),
                        matchRound.MatchHallName,
                        opponent,
                        matchRound.MatchDate.ToDateTime(matchRound.MatchTime),
                        matchRound.MatchNbrOfPlayers == 4,
                        OilPatternInformation.Create(matchRound.MatchOilPatternName, matchRound.MatchOilPatternId));
                    DocumentSession.Store(roster);
                }
            }

            return Ok();
        }

        private IHttpActionResult Handle(OneTimeKeyEvent @event)
        {
            const string Subject = "Logga in på Snittlistan";
            Emails.SendOneTimePassword(@event.Email, Subject, @event.OneTimePassword);
            return Ok();
        }

        private async Task<IHttpActionResult> Handle(VerifyMatchMessage message)
        {
            var roster = DocumentSession.Load<Roster>(message.RosterId);
            if (roster.IsVerified) return Ok();
            var players = DocumentSession.Query<Player, PlayerSearch>()
                                         .ToArray();
            var parser = new BitsParser(players);
            var websiteConfig = DocumentSession.Load<WebsiteConfig>(WebsiteConfig.GlobalId);
            var result = await bitsClient.GetHeadInfo(roster.BitsMatchId);
            var header = BitsParser.ParseHeader(result, websiteConfig.ClubId);

            // chance to update roster values
            roster.OilPattern = header.OilPattern;
            roster.Date = header.Date;
            roster.Opponent = header.Opponent;
            roster.Location = header.Location;
            if (roster.MatchResultId == null) return Ok();

            // update match result values
            var bitsMatchResult = await bitsClient.GetBitsMatchResult(roster.BitsMatchId);
            if (roster.IsFourPlayer)
            {
                var matchResult = EventStoreSession.Load<MatchResult4>(roster.MatchResultId);
                var parseResult = parser.Parse4(bitsMatchResult, websiteConfig.ClubId);
                roster.Players = GetPlayerIds(parseResult);
                var isVerified = matchResult.Update(
                    PublishMessage,
                    roster,
                    parseResult.TeamScore,
                    parseResult.OpponentScore,
                    roster.BitsMatchId,
                    parseResult.CreateMatchSeries(),
                    players);
                roster.IsVerified = isVerified;
            }
            else
            {
                var matchResult = EventStoreSession.Load<MatchResult>(roster.MatchResultId);
                var parseResult = parser.Parse(bitsMatchResult, websiteConfig.ClubId);
                roster.Players = GetPlayerIds(parseResult);
                var resultsForPlayer = DocumentSession.Query<ResultForPlayerIndex.Result, ResultForPlayerIndex>()
                                                      .Where(x => x.Season == roster.Season)
                                                      .ToArray()
                                                      .ToDictionary(x => x.PlayerId);
                var matchSeries = parseResult.CreateMatchSeries();
                var isVerified = matchResult.Update(
                    PublishMessage,
                    roster,
                    parseResult.TeamScore,
                    parseResult.OpponentScore,
                    matchSeries,
                    parseResult.OpponentSeries,
                    players,
                    resultsForPlayer);
                roster.IsVerified = isVerified;
            }

            return Ok();
        }

        private async Task<IHttpActionResult> Handle(RegisterMatchesMessage message)
        {
            var websiteConfig = DocumentSession.Load<WebsiteConfig>(WebsiteConfig.GlobalId);
            var pendingMatches = ExecuteQuery(new GetPendingMatchesQuery(websiteConfig.SeasonId));
            var players = ExecuteQuery(new GetActivePlayersQuery());
            var registeredMatches = new List<Roster>();
            foreach (var pendingMatch in pendingMatches)
            {
                try
                {
                    var parser = new BitsParser(players);
                    var bitsMatchResult = await bitsClient.GetBitsMatchResult(pendingMatch.BitsMatchId);
                    if (pendingMatch.IsFourPlayer)
                    {
                        var parse4Result = parser.Parse4(bitsMatchResult, websiteConfig.ClubId);
                        if (parse4Result != null)
                        {
                            var allPlayerIds = GetPlayerIds(parse4Result);
                            pendingMatch.Players = allPlayerIds;
                            ExecuteCommand(new RegisterMatch4Command(pendingMatch, parse4Result));
                        }
                    }
                    else
                    {
                        var parseResult = parser.Parse(bitsMatchResult, websiteConfig.ClubId);
                        if (parseResult != null)
                        {
                            var allPlayerIds = GetPlayerIds(parseResult);
                            pendingMatch.Players = allPlayerIds;
                            ExecuteCommand(new RegisterMatchCommand(pendingMatch, parseResult));
                        }
                    }

                    registeredMatches.Add(pendingMatch);
                }
                catch (Exception e)
                {
                    ErrorSignal
                        .FromCurrentContext()
                        .Raise(new Exception($"Unable to auto register match {pendingMatch.Id} ({pendingMatch.BitsMatchId})", e));
                }
            }

            var result = registeredMatches.Select(x => new
            {
                x.Date,
                x.Season,
                x.Turn,
                x.BitsMatchId,
                x.Team,
                x.Location,
                x.Opponent
            }).ToArray();
            if (result.Length > 0)
                return Ok(result);

            return Ok("No matches to register");
        }

        private static List<string> GetPlayerIds(Parse4Result parse4Result)
        {
            var query = from game in parse4Result.Series.First().Games
                        select game.Player;
            var playerIds = query.ToArray();
            var playerIdsWithoutReserve = new HashSet<string>(playerIds);
            var restQuery = from serie in parse4Result.Series
                            from game in serie.Games
                            where playerIdsWithoutReserve.Contains(game.Player) == false
                            select game.Player;
            var allPlayerIds = playerIds.Concat(
                new HashSet<string>(restQuery).Where(x => playerIdsWithoutReserve.Contains(x) == false)).ToList();
            return allPlayerIds;
        }

        private static List<string> GetPlayerIds(ParseResult parseResult)
        {
            var query = from table in parseResult.Series.First().Tables
                        from game in new[] { table.Game1, table.Game2 }
                        select game.Player;
            var playerIds = query.ToArray();
            var playerIdsWithoutReserve = new HashSet<string>(playerIds);
            var restQuery = from serie in parseResult.Series
                            from table in serie.Tables
                            from game in new[] { table.Game1, table.Game2 }
                            where playerIdsWithoutReserve.Contains(game.Player) == false
                            select game.Player;
            var allPlayerIds = playerIds.Concat(
                new HashSet<string>(restQuery).Where(x => playerIdsWithoutReserve.Contains(x) == false)).ToList();
            return allPlayerIds;
        }

        private IHttpActionResult Handle(InitializeIndexesMessage message)
        {
            IndexCreator.CreateIndexes(DocumentStore);

            return Ok();
        }

        private IHttpActionResult Handle(VerifyMatchesMessage message)
        {
            var season = DocumentSession.LatestSeasonOrDefault(SystemTime.UtcNow.Year);
            var rosters = DocumentSession.Query<Roster, RosterSearchTerms>()
                                         .Where(x => x.Season == season)
                                         .ToArray();
            var toVerify = new List<VerifyMatchMessage>();
            foreach (var roster in rosters)
            {
                if (roster.BitsMatchId == 0)
                {
                    continue;
                }

                if (roster.IsVerified)
                {
                    Log.Info($"Skipping {roster.BitsMatchId} because it is already verified.");
                }
                else
                {
                    toVerify.Add(new VerifyMatchMessage(roster.BitsMatchId, roster.Id));
                }
            }

            foreach (var verifyMatchMessage in toVerify)
            {
                PublishMessage(verifyMatchMessage);
            }

            return Ok();
        }

        private IHttpActionResult Handle(NewUserCreatedEvent @event)
        {
            var recipient = @event.Email;
            const string Subject = "Välkommen till Snittlistan!";
            var activationKey = @event.ActivationKey;
            var id = @event.UserId;

            Emails.UserRegistered(recipient, Subject, id, activationKey);

            return Ok();
        }

        private IHttpActionResult Handle(UserInvitedEvent @event)
        {
            var recipient = @event.Email;
            const string Subject = "Välkommen till Snittlistan!";
            var activationUri = @event.ActivationUri;

            Emails.InviteUser(recipient, Subject, activationUri);

            return Ok();
        }

        private IHttpActionResult Handle(EmailTask task)
        {
            Emails.SendMail(
                task.Recipient,
                Encoding.UTF8.GetString(Convert.FromBase64String(task.Subject)),
                Encoding.UTF8.GetString(Convert.FromBase64String(task.Content)));

            return Ok();
        }

        private IHttpActionResult Handle(MatchRegisteredEvent @event)
        {
            var roster = DocumentSession.Load<Roster>(@event.RosterId);
            if (roster.IsFourPlayer) return Ok();
            var resultSeriesReadModelId = ResultSeriesReadModel.IdFromBitsMatchId(roster.BitsMatchId, roster.Id);
            var resultSeriesReadModel = DocumentSession.Load<ResultSeriesReadModel>(resultSeriesReadModelId);
            var resultHeaderReadModelId = ResultHeaderReadModel.IdFromBitsMatchId(roster.BitsMatchId, roster.Id);
            var resultHeaderReadModel = DocumentSession.Load<ResultHeaderReadModel>(resultHeaderReadModelId);
            Emails.MatchRegistered(
                roster.Team,
                roster.Opponent,
                @event.Score,
                @event.OpponentScore,
                resultSeriesReadModel,
                resultHeaderReadModel);

            return Ok();
        }

        public class TaskRequest
        {
            public TaskRequest(string taskJson)
            {
                TaskJson = taskJson;
            }

            [Required]
            public string TaskJson { get; }
        }
    }
}
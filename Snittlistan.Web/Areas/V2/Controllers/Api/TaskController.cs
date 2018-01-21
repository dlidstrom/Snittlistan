using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Http;
using Elmah;
using Newtonsoft.Json;
using NLog;
using Raven.Abstractions;
using Snittlistan.Queue.Messages;
using Snittlistan.Web.Areas.V2.Commands;
using Snittlistan.Web.Areas.V2.Domain;
using Snittlistan.Web.Areas.V2.Domain.Match;
using Snittlistan.Web.Areas.V2.Indexes;
using Snittlistan.Web.Areas.V2.Queries;
using Snittlistan.Web.Areas.V2.ReadModels;
using Snittlistan.Web.Controllers;
using Snittlistan.Web.Helpers;
using Snittlistan.Web.Infrastructure;
using Snittlistan.Web.Infrastructure.Attributes;
using Snittlistan.Web.Infrastructure.Indexes;
using Snittlistan.Web.Models;

namespace Snittlistan.Web.Areas.V2.Controllers.Api
{
    [OnlyLocalAllowed]
    public class TaskController : AbstractApiController
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private readonly IBitsClient bitsClient;

        public TaskController(IBitsClient bitsClient)
        {
            this.bitsClient = bitsClient;
        }

        public IHttpActionResult Post(TaskRequest request)
        {
            if (ModelState.IsValid == false) return BadRequest(ModelState);
            dynamic task = JsonConvert.DeserializeObject(
                request.TaskJson,
                new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All });
            var result = Handle(task);
            return result;
        }

        private IHttpActionResult Handle(VerifyMatchMessage message)
        {
            var result = bitsClient.DownloadMatchResult(message.BitsMatchId);
            var roster = DocumentSession.Include<Roster>(x => x.Players)
                                        .Load<Roster>(message.RosterId);
            if (roster.IsVerified) return Ok();
            var players = roster.Players
                                .Select(x => DocumentSession.Load<Player>(x))
                                .ToArray();
            var parser = new BitsParser(players);
            var websiteConfig = DocumentSession.Load<WebsiteConfig>(WebsiteConfig.GlobalId);
            var header = BitsParser.ParseHeader(result, websiteConfig.GetTeamNames());

            // chance to update roster values
            roster.OilPattern = header.OilPattern;
            roster.Date = header.Date;
            if (roster.MatchResultId == null) return Ok();

            // update match result values
            if (roster.IsFourPlayer)
            {
                var matchResult = EventStoreSession.Load<MatchResult4>(roster.MatchResultId);
                var parseResult = parser.Parse4(result, roster.Team);
                var isVerified = matchResult.Update(
                    PublishMessage,
                    roster,
                    parseResult.TeamScore,
                    parseResult.OpponentScore,
                    roster.BitsMatchId,
                    parseResult.CreateMatchSeries());
                roster.IsVerified = isVerified;
            }
            else
            {
                var matchResult = EventStoreSession.Load<MatchResult>(roster.MatchResultId);
                var parseResult = parser.Parse(result, roster.Team);
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

        private IHttpActionResult Handle(RegisterMatchesMessage message)
        {
            var pendingMatches = ExecuteQuery(new GetPendingMatchesQuery());
            var players = ExecuteQuery(new GetActivePlayersQuery());
            var registeredMatches = new List<Roster>();
            foreach (var pendingMatch in pendingMatches)
            {
                try
                {
                    var parser = new BitsParser(players);
                    var content = bitsClient.DownloadMatchResult(pendingMatch.BitsMatchId);
                    if (pendingMatch.IsFourPlayer)
                    {
                        var parse4Result = parser.Parse4(content, pendingMatch.Team);
                        if (parse4Result != null)
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
                            pendingMatch.Players = allPlayerIds;
                            ExecuteCommand(new RegisterMatch4Command(pendingMatch, parse4Result));
                        }
                    }
                    else
                    {
                        var parseResult = parser.Parse(content, pendingMatch.Team);
                        if (parseResult != null)
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
            Emails.SendMail(task.Recipient, task.Subject, task.Content);

            return Ok();
        }

        private IHttpActionResult Handle(MatchRegisteredEvent @event)
        {
            var roster = DocumentSession.Load<Roster>(@event.RosterId);
            if (roster.IsFourPlayer) return Ok();
            var resultSeriesReadModelId = ResultSeriesReadModel.IdFromBitsMatchId(roster.BitsMatchId);
            var resultSeriesReadModel = DocumentSession.Load<ResultSeriesReadModel>(resultSeriesReadModelId);
            var resultHeaderReadModelId = ResultHeaderReadModel.IdFromBitsMatchId(roster.BitsMatchId);
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
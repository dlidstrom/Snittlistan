using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Http;
using Elmah;
using Newtonsoft.Json;
using NLog;
using Raven.Abstractions;
using Snittlistan.Queue;
using Snittlistan.Queue.Messages;
using Snittlistan.Web.Areas.V2.Commands;
using Snittlistan.Web.Areas.V2.Domain;
using Snittlistan.Web.Areas.V2.Domain.Match;
using Snittlistan.Web.Areas.V2.Indexes;
using Snittlistan.Web.Areas.V2.Queries;
using Snittlistan.Web.Controllers;
using Snittlistan.Web.Helpers;
using Snittlistan.Web.Infrastructure;
using Snittlistan.Web.Infrastructure.Attributes;
using Snittlistan.Web.Infrastructure.Indexes;

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
            if (roster.IsFourPlayer)
            {
                var matchResult = EventStoreSession.Load<MatchResult4>(roster.MatchResultId);
                var parseResult = parser.Parse4(result, roster.Team);
                var isVerified = matchResult.Update(
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

            var registeredMatches = new List<Roster>();
            foreach (var pendingMatch in pendingMatches)
            {
                try
                {
                    var players = ExecuteQuery(new GetPlayersQuery(pendingMatch));
                    var parser = new BitsParser(players);
                    var content = bitsClient.DownloadMatchResult(pendingMatch.BitsMatchId);
                    if (pendingMatch.IsFourPlayer)
                    {
                        var parse4Result = parser.Parse4(content, pendingMatch.Team);
                        ExecuteCommand(new RegisterMatch4Command(pendingMatch, parse4Result));
                    }
                    else
                    {
                        var parseResult = parser.Parse(content, pendingMatch.Team);
                        ExecuteCommand(new RegisterMatchCommand(pendingMatch, parseResult));
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
            using (var scope = MsmqGateway.AutoCommitScope())
            {
                foreach (var roster in rosters)
                {
                    if (roster.IsVerified)
                    {
                        Log.Info($"Skipping {roster.BitsMatchId} because it is already verified.");
                    }
                    else if (roster.BitsMatchId == 0)
                    {
                        Log.Info($"Skipping {roster.Team}-{roster.Opponent} (turn={roster.Turn}) because it has no BitsMatchId.");
                    }
                    else if (roster.MatchResultId == null)
                    {
                        Log.Info($"Skipping {roster.BitsMatchId} because it has no result yet.");
                    }
                    else
                    {
                        Log.Info($"Need to verify {roster.BitsMatchId}");
                        var verifyMatchMessage = new VerifyMatchMessage(roster.BitsMatchId, roster.Id);
                        var envelope = new MessageEnvelope(message, new Uri(Url.Link("DefaultApi", new { controller = "Task" })));
                        scope.PublishMessage(envelope);
                    }
                }
            }

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
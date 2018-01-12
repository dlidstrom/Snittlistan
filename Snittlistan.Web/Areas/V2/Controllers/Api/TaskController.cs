using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Http;
using Elmah;
using Newtonsoft.Json;
using Snittlistan.Queue.Messages;
using Snittlistan.Web.Areas.V2.Commands;
using Snittlistan.Web.Areas.V2.Domain;
using Snittlistan.Web.Areas.V2.Domain.Match;
using Snittlistan.Web.Areas.V2.Indexes;
using Snittlistan.Web.Areas.V2.Queries;
using Snittlistan.Web.Controllers;
using Snittlistan.Web.Infrastructure;

namespace Snittlistan.Web.Areas.V2.Controllers.Api
{
    public class TaskController : AbstractApiController
    {
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
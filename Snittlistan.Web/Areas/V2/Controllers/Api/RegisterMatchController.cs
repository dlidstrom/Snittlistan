using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using Elmah;
using Snittlistan.Web.Areas.V2.Commands;
using Snittlistan.Web.Areas.V2.Domain;
using Snittlistan.Web.Areas.V2.Queries;
using Snittlistan.Web.Controllers;
using Snittlistan.Web.Infrastructure;
using ApplicationException = System.ApplicationException;

namespace Snittlistan.Web.Areas.V2.Controllers.Api
{
    public class RegisterMatchController : AbstractApiController
    {
        private readonly IBitsClient bitsClient;

        public RegisterMatchController(IBitsClient bitsClient)
        {
            this.bitsClient = bitsClient;
        }

        public HttpResponseMessage Get()
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
                    var message = $"Unable to auto register match {pendingMatch.Id} ({pendingMatch.BitsMatchId})";
                    ErrorSignal
                        .FromCurrentContext()
                        .Raise(new ApplicationException(message, e));
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
                return Request.CreateResponse(HttpStatusCode.OK, result);

            return Request.CreateResponse(HttpStatusCode.OK, "No matches to register");
        }
    }
}
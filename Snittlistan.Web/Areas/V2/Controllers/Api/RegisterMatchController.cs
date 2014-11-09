using System.Linq;
using System.Net;
using System.Net.Http;
using Snittlistan.Web.Areas.V2.Domain;
using Snittlistan.Web.Areas.V2.Queries;
using Snittlistan.Web.Controllers;
using Snittlistan.Web.Infrastructure;

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

            foreach (var pendingMatch in pendingMatches)
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
            }

            var result = pendingMatches.Select(x => new
            {
                x.Date,
                x.Season,
                x.Turn,
                x.BitsMatchId,
                x.Team,
                x.Location,
                x.Opponent
            });
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}
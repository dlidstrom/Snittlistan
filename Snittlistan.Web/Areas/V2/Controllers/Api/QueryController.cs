
using System.ComponentModel.DataAnnotations;
using System.Web.Http;
using Snittlistan.Web.Infrastructure.Attributes;
using Snittlistan.Web.Models;
using Newtonsoft.Json;
using Snittlistan.Queue.Queries;
using Snittlistan.Web.Controllers;

#nullable enable

namespace Snittlistan.Web.Areas.V2.Controllers.Api;
[OnlyLocalAllowed]
public class QueryController : AbstractApiController
{
    public IHttpActionResult Post(QueryRequest request)
    {
        dynamic? query = JsonConvert.DeserializeObject(
            request.QueryJson,
            new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All });
        dynamic result = Handle(query);
        return result;
    }

    private IHttpActionResult Handle(GetTeamNamesQuery query)
    {
        WebsiteConfig websiteConfig = DocumentSession.Load<WebsiteConfig>(WebsiteConfig.GlobalId);
        GetTeamNamesQuery.TeamNameAndLevel[] teamNameAndLevels =
            websiteConfig.TeamNamesAndLevels
                .Select(x => new GetTeamNamesQuery.TeamNameAndLevel(x.TeamName, x.Level))
                .ToArray();
        GetTeamNamesQuery.Result result = new(teamNameAndLevels);
        return Ok(result);
    }

    public class QueryRequest
    {
        public QueryRequest(string queryJson)
        {
            QueryJson = queryJson;
        }

        [Required]
        public string QueryJson { get; }
    }
}

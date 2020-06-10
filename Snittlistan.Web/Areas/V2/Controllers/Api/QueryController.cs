namespace Snittlistan.Web.Areas.V2.Controllers.Api
{
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web.Http;
    using Infrastructure.Attributes;
    using Models;
    using Newtonsoft.Json;
    using Queue.Queries;
    using Web.Controllers;

    [OnlyLocalAllowed]
    public class QueryController : AbstractApiController
    {
        public IHttpActionResult Post(QueryRequest request)
        {
            if (ModelState.IsValid == false) return BadRequest(ModelState);
            dynamic query = JsonConvert.DeserializeObject(
                request.QueryJson,
                new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All });
            dynamic result = Handle(query);
            return result;
        }

        private IHttpActionResult Handle(GetTeamNamesQuery query)
        {
            WebsiteConfig websiteConfig = DocumentSession.Load<WebsiteConfig>(WebsiteConfig.GlobalId);
            GetTeamNamesQuery.TeamNameAndLevel[] teamNameAndLevels = websiteConfig.TeamNamesAndLevels
                                                 .Select(x => new GetTeamNamesQuery.TeamNameAndLevel(x.TeamName, x.Level))
                                                 .ToArray();
            var result = new GetTeamNamesQuery.Result(teamNameAndLevels);
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
}
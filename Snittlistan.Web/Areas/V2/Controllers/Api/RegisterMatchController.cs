using System.Net;
using System.Net.Http;
using Snittlistan.Web.Controllers;

namespace Snittlistan.Web.Areas.V2.Controllers.Api
{
    public class RegisterMatchController : AbstractApiController
    {
        public HttpResponseMessage Get()
        {
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
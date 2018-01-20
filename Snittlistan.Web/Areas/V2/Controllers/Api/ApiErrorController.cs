using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Snittlistan.Web.Areas.V2.Controllers.Api
{
    public class ApiErrorController : ApiController
    {
        [HttpGet, HttpPost, HttpPut, HttpDelete, HttpHead, HttpOptions, AcceptVerbs("PATCH")]
        public HttpResponseMessage Handle404()
        {
            var responseMessage = new HttpResponseMessage(HttpStatusCode.NotFound)
            {
                ReasonPhrase = "The requested resource is not found"
            };
            return responseMessage;
        }
    }
}
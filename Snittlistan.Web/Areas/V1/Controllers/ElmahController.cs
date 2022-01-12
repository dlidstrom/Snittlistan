
using System.Web.Mvc;
using Snittlistan.Web.Infrastructure.Results;

namespace Snittlistan.Web.Areas.V1.Controllers;
[Authorize]
public class ElmahController : Controller
{
    public ActionResult Index(string type)
    {
        return new ElmahResult(type);
    }
}

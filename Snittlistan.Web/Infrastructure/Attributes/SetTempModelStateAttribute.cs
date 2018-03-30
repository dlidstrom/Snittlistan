namespace Snittlistan.Web.Infrastructure.Attributes
{
    using System.Web.Mvc;

    public class SetTempModelStateAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            filterContext.Controller.TempData["ModelState"] = filterContext.Controller.ViewData.ModelState;
        }
    }
}
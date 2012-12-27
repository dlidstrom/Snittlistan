namespace Snittlistan.Web.Helpers
{
    using System.IO;
    using System.Web.Mvc;
    using System.Web.Routing;

    using Snittlistan.Web.Areas.V1.Controllers;
    using Snittlistan.Web.Infrastructure;

    public static class ViewHelper
    {
        public static string RenderEmailBody(object viewModel, string viewName)
        {
            var routeData = new RouteData();
            routeData.Values.Add("controller", "MailTemplates");
            var controllerContext = new ControllerContext(new MailHttpContext(), routeData, new MailController());
            var viewEngineResult = ViewEngines.Engines.FindView(controllerContext, viewName, "_Layout");
            var stringWriter = new StringWriter();
            viewEngineResult.View.Render(
                new ViewContext(
                    controllerContext,
                    viewEngineResult.View,
                    new ViewDataDictionary(viewModel),
                    new TempDataDictionary(),
                    stringWriter),
                stringWriter);

            return stringWriter.GetStringBuilder().ToString();
        }
    }
}
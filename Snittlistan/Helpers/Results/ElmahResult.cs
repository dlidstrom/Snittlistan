namespace Snittlistan.Helpers.Results
{
	using System.Web;
	using System.Web.Mvc;
	using Elmah;

	public class ElmahResult : ActionResult
	{
		private string resourceType;

		public ElmahResult(string resourceType)
		{
			this.resourceType = resourceType;
		}

		public override void ExecuteResult(ControllerContext context)
		{
			var url = new UrlHelper(HttpContext.Current.Request.RequestContext);
			var factory = new ErrorLogPageFactory();
			if (!string.IsNullOrEmpty(resourceType))
			{
				var pathInfo = "." + resourceType;
				var action = url.Action("Index", "Elmah", new { type = (string)null });
				HttpContext.Current.RewritePath(action, pathInfo, HttpContext.Current.Request.QueryString.ToString());
			}
			
			var handler = factory.GetHandler(HttpContext.Current, null, null, null);

			handler.ProcessRequest(HttpContext.Current);
		}
	}
}
namespace Snittlistan.Infrastructure
{
    using System.Web.Mvc;
    using Raven.Client;

    /// <summary>
	/// This filter makes sure to save any outstanding changes.
	/// It does so by calling SaveChanges on the current session,
	/// unless there was an error in the action.
	/// </summary>
	public class RavenActionFilterAttribute : ActionFilterAttribute
	{
        public IDocumentSession Session { get; set; }

		public override void OnActionExecuted(ActionExecutedContext filterContext)
		{
			// save changes unless there was an exception
			if (!filterContext.IsChildAction && filterContext.Exception == null)
			{
				Session.SaveChanges();
			}
		}
	}
}
using System.Web.Mvc;
using Raven.Client;

namespace Snittlistan.Controllers
{
	[Authorize]
	public abstract class AdminController : AbstractController
	{
		public AdminController(IDocumentSession session)
			: base(session)
		{ }
	}
}
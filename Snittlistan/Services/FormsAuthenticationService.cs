using System.Web.Security;

namespace SnittListan.Services
{
	public class FormsAuthenticationService : IAuthenticationService
	{
		public void SetAuthCookie(string userName, bool createPersistentCookie)
		{
			FormsAuthentication.SetAuthCookie(userName, createPersistentCookie);
		}

		public void SignOut()
		{
			FormsAuthentication.SignOut();
		}
	}
}
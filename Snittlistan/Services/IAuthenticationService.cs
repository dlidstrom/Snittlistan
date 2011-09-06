namespace Snittlistan.Services
{
	public interface IAuthenticationService
	{
		void SetAuthCookie(string userName, bool createPersistentCookie);

		void SignOut();
	}
}
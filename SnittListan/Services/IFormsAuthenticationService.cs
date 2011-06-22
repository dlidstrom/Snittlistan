namespace SnittListan.Services
{
	public interface IFormsAuthenticationService
	{
		void SetAuthCookie(string userName, bool createPersistentCookie);
	}
}

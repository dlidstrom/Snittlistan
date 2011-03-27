namespace SnittListan.Infrastructure
{
	using System;
	using System.Web.Security;
	using SnittListan.Services;

	/// <summary>
	/// Implementation of the forms authentication service.
	/// </summary>
	public class FormsAuthenticationService : IFormsAuthenticationService
	{
		/// <summary>
		/// Creates an authentication ticket for the supplied user name and adds it to
		/// the cookies collection of the response, or to the URL if you are using cookieless
		/// authentication.
		/// </summary>
		/// <param name="userName">The name of an authenticated user. This does not have to map to a Windows
		/// account.</param>
		/// <param name="createPersistentCookie">True to create a persistent cookie (one that
		/// is saved across browser sessions); otherwise, false.</param>
		/// <exception cref="System.Web.HttpException">System.Web.Security.FormsAuthentication.RequireSSL is
		/// true and System.Web.HttpRequest.IsSecureConnection is false.</exception>
		public void SignIn(string userName, bool createPersistentCookie)
		{
			if (string.IsNullOrEmpty(userName))
			{
				throw new ArgumentException("Value cannot be null or empty.", "userName");
			}

			FormsAuthentication.SetAuthCookie(userName, createPersistentCookie);
		}

		/// <summary>
		/// Removes the forms-authentication ticket from the browser.
		/// </summary>
		public void SignOut()
		{
			FormsAuthentication.SignOut();
		}
	}
}
namespace Snittlistan.Web.Services
{
    /// <summary>
    /// Represents an authentication services.
    /// </summary>
    public interface IAuthenticationService
    {
        /// <summary>
        /// Create an authentication ticket for the supplied user name.
        /// </summary>
        /// <param name="userName">The name of an authenticated user.</param>
        /// <param name="createPersistentCookie">True to create a persistent cookie (one that is saved across browser sessions);
        /// otherwise, false.</param>
        void SetAuthCookie(string userName, bool createPersistentCookie);

        /// <summary>
        /// Remove the authentication ticket from the browser.
        /// </summary>
        void SignOut();
    }
}
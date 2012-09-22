namespace Snittlistan.Web.Events
{
    using Snittlistan.Web.Models;

    /// <summary>
    /// Raised when a new user is created.
    /// </summary>
    public class NewUserCreatedEvent : IDomainEvent
    {
        /// <summary>
        /// Gets or sets the user.
        /// </summary>
        public User User { get; set; }
    }
}